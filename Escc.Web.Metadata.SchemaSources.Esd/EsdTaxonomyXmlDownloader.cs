using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using Escc.Net;
using Escc.Web.Metadata;

namespace Escc.Egms.SchemaSources.Esd
{
    /// <summary>
    /// Download updates to ESD lists in Taxonomy XML format
    /// </summary>
    public class EsdTaxonomyXmlDownloader
    {
        /// <summary>
        /// Checks for a newer XML file and, if found, backs up and replaces the old copy
        /// </summary>
        /// <param name="currentFile">The path of the current XML file</param>
        /// <returns>true if old version replaced; false otherwise</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown is NTFS denies permission write to the XML file</exception>
        /// <exception cref="System.Security.SecurityException">Thrown if the CLR denies permission to access the Internet or to write to the XML file</exception>
        /// <exception cref="System.Net.WebException">Thrown if there was a problem accessing the Internet</exception>
        /// <exception cref="System.Xml.XmlException">Thrown is XML downloaded from web is not well formed</exception>
        /// <exception cref="System.FormatException">Thrown if the list or mapping version is not recognised as a number</exception>
        public bool CheckForNewXml(string currentFile)
        {
            currentFile = new Uri(currentFile).LocalPath;
            
            var currentXml = EsdXmlDocument.GetEsdDocument(new Uri(currentFile, UriKind.RelativeOrAbsolute));

            EsdXmlDocument latestXml = this.GetXmlFromWeb(currentXml.LatestVersionUrl);

            // Version number is not consistently specified, so be prepared for errors
            float currentVersion = 0;
            float latestVersion = 0;
            try
            {
                currentVersion = currentXml.Version;
            }
            catch (FormatException ex)
            {
                ex.Data.Add("Controlled list URL", currentXml.ThisVersionUrl.ToString());
                throw;
            }
            try
            {
                latestVersion = latestXml.Version;
            }
            catch (FormatException ex)
            {
                ex.Data.Add("Controlled list URL", currentXml.LatestVersionUrl.ToString());
                throw;
            }

            // Mappings aren't versioned any more, so always download them. Continue to check version for lists.
            if (latestVersion > currentVersion || latestXml is EsdMapping)
            {
                FileIOPermission xmlWritePermission = new FileIOPermission(FileIOPermissionAccess.Write, Path.GetDirectoryName(currentFile));
                xmlWritePermission.Demand();

                // set up the XML writer and namespaces
                string tempFile = String.Format(CultureInfo.InvariantCulture, "{0}.temp", currentFile);

                XmlTextWriter writer = new XmlTextWriter(tempFile, System.Text.Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                latestXml.WriteContentTo(writer);
                writer.Flush();
                writer.Close();

                // If backup of current file succeeds, copy downloaded XML over current file and delete temp file
                if (this.BackupXmlFile(currentFile))
                {
                    File.Copy(tempFile, currentFile, true);
                    File.Delete(tempFile);

                    return true;
                }
                else
                {
                    File.Delete(tempFile);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the latest version of the XML for an ESD controlled list from the web
        /// </summary>
        /// <param name="url">The URL to download from, typically taken from the LatestVersionUrl in the current data</param>
        /// <returns>The downloaded list, or null</returns>
        /// <exception cref="System.Security.SecurityException">Thrown if the CLR denies permission to access the Internet</exception>
        /// <exception cref="System.Net.WebException">Thrown if there was a problem accessing the Internet</exception>
        /// <exception cref="System.Xml.XmlException">Thrown is XML downloaded from web is not well formed</exception>
        /// <remarks>To use a proxy server, add its IP to the appSettings section of the application configuration file, in an element with a key named "ProxyServer". 
        /// It will use the current Windows credentials. To use different credentials, add extra elements with the following keys: "ProxyUser", "ProxyPassword" and "ProxyDomain".</remarks>
        public EsdXmlDocument GetXmlFromWeb(Uri url)
        {
            WebPermission xmlWebPermission = new WebPermission(NetworkAccess.Connect, url.ToString());
            xmlWebPermission.Demand();

            var client = new HttpRequestClient(new ConfigurationProxyProvider());
            WebRequest xmlRequest = client.CreateRequest(url);
            WebResponse xmlResponse = xmlRequest.GetResponse();

            try
            {
                if (this.GetType() == typeof(EsdControlledList))
                {
                    EsdControlledList downloadedDoc = new EsdControlledList();
                    downloadedDoc.LoadDom(xmlResponse.GetResponseStream());
                    return downloadedDoc;
                }
                else if (this.GetType() == typeof(EsdMapping))
                {
                    EsdMapping downloadedDoc = new EsdMapping();
                    downloadedDoc.LoadDom(xmlResponse.GetResponseStream());
                    return downloadedDoc;
                }
                else return null;
            }
            catch (XmlException ex)
            {
                // if the XML's invalid, repeat the request so the XML can be included in the error
                xmlRequest = client.CreateRequest(url);
                using (StreamReader r = new StreamReader(xmlRequest.GetResponse().GetResponseStream()))
                {
                    ex.Data["XML"] = r.ReadToEnd();
                }
                throw;
            }
        }

        /// <summary>
        /// Backs up an XML file to a copy with the version number
        /// </summary>
        /// <param name="currentFile">The path of the current XML file</param>
        /// <returns>true if copy successful; false if permission denied</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown is NTFS denies permission write to the XML file</exception>
        /// <exception cref="System.Security.SecurityException">Thrown if the CLR denies permission to write to the XML file</exception>
        private bool BackupXmlFile(string currentFile)
        {
            FileIOPermission xmlWritePermission = new FileIOPermission(FileIOPermissionAccess.Write, Path.GetDirectoryName(currentFile));
            xmlWritePermission.Demand();

            var currentXml = EsdXmlDocument.GetEsdDocument(new Uri(currentFile, UriKind.RelativeOrAbsolute));

            string currentVersion = currentXml.Version.ToString(CultureInfo.CurrentCulture);
            string newName = String.Format(CultureInfo.InvariantCulture, "{0}-v{1}.backup{2}", Path.GetFileNameWithoutExtension(currentFile), currentVersion, Path.GetExtension(currentFile));
            string backupFolder = Path.GetDirectoryName(currentFile) + Path.DirectorySeparatorChar + "backup" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(backupFolder)) Directory.CreateDirectory(backupFolder);
            File.Copy(currentFile, backupFolder + newName, true);

            return true;
        }
    }
}
