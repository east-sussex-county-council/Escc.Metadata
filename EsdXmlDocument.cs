using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using EsccWebTeam.Data.Xml;
using EsccWebTeam.Egms.Properties;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
    /// <summary>
    /// An XML representation of an ESD standard
    /// </summary>
    public abstract class EsdXmlDocument
    {
        #region Private member vars
        private XmlDocument xmlDocument;
        private XPathDocument xpathDocument;
        private XmlNamespaceManager nsManager;
        private bool domDataLoaded;
        private bool xpathDataLoaded;
        private string xmlFile;

        #endregion // Private member vars

        #region Static factory methods
        /// <summary>
        /// Gets the XML for an ESD controlled list or mapping, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;EsccWebTeam.Egms/ControlledListXml&gt; section of web.config</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdXmlDocument GetEsdDocument(string configKey)
        {
            return EsdXmlDocument.GetEsdDocument(configKey, false);
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list or mapping, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;EsccWebTeam.Egms/ControlledListXml&gt; section of web.config</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdXmlDocument GetEsdDocument(string configKey, bool withDom)
        {
            // TODO: Ideally this would check the format of the file to decide, but as a quick fix
            // just check whether the key includes the word "mapping"

            NameValueCollection config = ConfigurationManager.GetSection("EsccWebTeam.Egms/ControlledListXml") as NameValueCollection;
            if (config == null) config = ConfigurationManager.GetSection("egmsXml") as NameValueCollection;
            if (config != null)
            {
                if (configKey.ToLower(CultureInfo.CurrentCulture).IndexOf("mapping") > -1)
                {
                    return EsdMapping.GetMapping(configKey, withDom);
                }
                else
                {
                    return EsdControlledList.GetControlledList(configKey, withDom);
                }
            }
            else return null;
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list or mapping, cacheing it if possible
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <returns>
        /// Populated XML document, or null if not found
        /// </returns>
        public static EsdXmlDocument GetEsdDocument(Uri xmlFileUri)
        {
            return EsdXmlDocument.GetEsdDocument(xmlFileUri, false);
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list or mapping, cacheing it if possible
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file doesn't exist</exception>
        /// <exception cref="XmlException">Thrown if the file isn't XML</exception>
        /// <exception cref="ArgumentException">Thrown if the file isn't recognised as ESD Taxonomy XML</exception>
        public static EsdXmlDocument GetEsdDocument(Uri xmlFileUri, bool withDom)
        {
            Type documentType = EsdXmlDocument.GetEsdDocumentType(xmlFileUri);
            if (documentType == typeof(EsdControlledList))
            {
                return EsdControlledList.GetControlledList(xmlFileUri, withDom);
            }
            else if (documentType == typeof(EsdMapping))
            {
                return EsdMapping.GetMapping(xmlFileUri, withDom);
            }

            // Need this to compile. Shouldn't ever return null, because if neither of those two match an exception should have been thrown
            return null;
        }

        /// <summary>
        /// Gets the type of the ESD Taxonomy XML document, either <seealso cref="EsdControlledList"/> or <seealso cref="EsdMapping"/>.
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <returns>Either <seealso cref="EsdControlledList"/> or <seealso cref="EsdMapping"/></returns>
        /// <exception cref="FileNotFoundException">Thrown if the file doesn't exist</exception>
        /// <exception cref="XmlException">Thrown if the file isn't XML</exception>
        /// <exception cref="ArgumentException">Thrown if the file isn't recognised as ESD Taxonomy XML</exception>
        public static Type GetEsdDocumentType(Uri xmlFileUri)
        {
            using (XmlReader r = XmlReader.Create(xmlFileUri.ToString()))
            {
                r.MoveToContent();
                if (r.NodeType == XmlNodeType.Element)
                {
                    if (r.Name == "ControlledList") return typeof(EsdControlledList);
                    else if (r.Name == "ItemMappings") return typeof(EsdMapping);
                }
            }
            throw new ArgumentException(Resources.ErrorNotTaxonomyXml, "xmlFileUri");
        }

        #endregion // Static factory methods


        #region Load XML data
        /// <summary>
        /// Gets whether data has been loaded with DOM support, and is available for XPath and DOM queries
        /// </summary>
        protected bool DomDataLoaded
        {
            get
            {
                return this.domDataLoaded;
            }
        }

        /// <summary>
        /// Gets whether data has been loaded and is available for XPath queries
        /// </summary>
        protected bool XPathDataLoaded
        {
            get
            {
                return this.xpathDataLoaded;
            }
        }

        /// <summary>
        /// Gets or sets the name and path of the file where the XML is currently stored
        /// </summary>
        public string XmlFile
        {
            set { this.xmlFile = value; }
            get { return this.xmlFile; }
        }

        /// <summary>
        /// Loads the XML document from the specified string. Use <seealso cref="GetEsdDocument(Uri, bool)"/> or <seealso cref="GetEsdDocument(string)"/> instead to ensure compatibility.
        /// </summary>
        /// <param name="xml">Well-formed XML data</param>
        public void LoadXmlDom(string xml)
        {
            if (this.xmlDocument == null) this.xmlDocument = new XmlDocument();
            this.xmlDocument.LoadXml(xml);
            this.xpathDocument = null;
            this.xpathDataLoaded = true;
            this.domDataLoaded = true;
        }

        /// <summary>
        /// Loads the XML document from the specified URL. Use <seealso cref="GetEsdDocument(Uri, bool)"/> or <seealso cref="GetEsdDocument(string)"/> instead to ensure compatibility.
        /// </summary>
        /// <param name="fileName">URL for the file containing the XML document to load</param>
        public void LoadDom(string fileName)
        {
            if (this.xmlDocument == null) this.xmlDocument = new XmlDocument();
            this.xmlDocument.Load(fileName);
            this.xpathDocument = null;
            this.xpathDataLoaded = true;
            this.domDataLoaded = true;
            this.xmlFile = fileName;
        }


        /// <summary>
        /// Loads the XML document from the specified stream. Use <seealso cref="GetEsdDocument(Uri, bool)"/> or <seealso cref="GetEsdDocument(string)"/> instead to ensure compatibility.
        /// </summary>
        /// <param name="inStream">The Stream containing the XML document to load</param>
        public void LoadDom(Stream inStream)
        {
            if (this.xmlDocument == null) this.xmlDocument = new XmlDocument();
            this.xmlDocument.Load(inStream);
            this.xpathDocument = null;
            this.xpathDataLoaded = true;
            this.domDataLoaded = true;
        }

        /// <summary>
        /// Loads XML data for querying from the specified URI. Use <seealso cref="GetEsdDocument(Uri, bool)"/> or <seealso cref="GetEsdDocument(string)"/> instead to ensure compatibility.
        /// </summary>
        /// <param name="uri">A URI that specifies a file containing the data to load</param>
        public void LoadXPath(string uri)
        {
            this.xpathDocument = new XPathDocument(uri);
            this.xmlDocument = null;
            this.xpathDataLoaded = true;
            this.domDataLoaded = false;
            this.xmlFile = uri;
        }


        #endregion // Load XML data

        #region DOM-only methods to retrieve data in XML format
        /// <summary>
        /// Gets the ESD XML for the document
        /// </summary>
        public string OuterXml
        {
            get
            {
                if (!this.DomDataLoaded) throw new NotSupportedException(Resources.ErrorDomNotLoaded);

                return this.xmlDocument.OuterXml;
            }
        }

        /// <summary>
        /// Saves the ESD XML document to the specified System.Xml.XmlWriter
        /// </summary>
        /// <param name="writer">The XmlWriter to which you want to save</param>
        public void WriteContentTo(XmlWriter writer)
        {
            if (!this.DomDataLoaded) throw new NotSupportedException(Resources.ErrorDomNotLoaded);

            this.xmlDocument.WriteContentTo(writer);
        }


        #endregion // DOM-only methods to retrieve data in XML format

        #region Select data using XPath

        /// <summary>
        /// Gets a namespace manager which sets up the ESD namespace with an "ns" prefix
        /// </summary>
        protected XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                if (this.nsManager == null)
                {
                    XPathNavigator nav = this.Xml.CreateNavigator();
                    this.nsManager = new XmlNamespaceManager(nav.NameTable);
                    nav.MoveToRoot();
                    nav.MoveToFirstChild();
                    this.nsManager.AddNamespace("ns", nav.NamespaceURI);
                }

                return this.nsManager;
            }
        }

        /// <summary>
        /// Gets the XmlDocument containing the ESD XML
        /// </summary>
        protected IXPathNavigable Xml
        {
            get
            {
                if (this.xmlDocument != null) return this.xmlDocument;
                else if (this.xpathDocument != null) return this.xpathDocument;
                else return this.xmlDocument;
            }
        }


        /// <summary>
        /// Selects nodes from the ESD XML matching the supplied XPath expression
        /// </summary>
        /// <param name="xpath">The XPath expression to evaluate</param>
        /// <returns></returns>
        protected XPathNodeIterator SelectNodes(string xpath)
        {
            if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

            XPathNavigator nav = this.Xml.CreateNavigator();
            nav.MoveToRoot();
            nav.MoveToFirstChild();

            return this.SelectNodes(xpath, nav);
        }

        /// <summary>
        /// Selects nodes from the ESD XML matching the supplied XPath expression
        /// </summary>
        /// <param name="xpath">The XPath expression to evaluate</param>
        /// <param name="nav">The position in the document to select from</param>
        /// <returns></returns>
        protected XPathNodeIterator SelectNodes(string xpath, XPathNavigator nav)
        {
            if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

            XPathExpression expr = nav.Compile(xpath);
            expr.SetContext(this.NamespaceManager);
            return nav.Select(expr);
        }
        #endregion // Select data using XPath

        #region Properties encapsulating specific XML data

        /// <summary>
        /// Gets the URL where the latest version of this document can be found online
        /// </summary>
        public Uri LatestVersionUrl
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();
                try
                {
                    return new Uri(nav.GetAttribute("LatestVersionLocation", ""));
                }
                catch (UriFormatException)
                {
                    // ESD standards are often published with bad attribute values. If the latest version URI isn't valid,
                    // try using the current version's URI and removing the version number.
                    // E-GMS browser up to and including 3.0 does not have this fix.
                    return new Uri(Regex.Replace(this.ThisVersionUrl.ToString(), "/" + this.Version.ToString(CultureInfo.InvariantCulture) + "(.0)?/", "/"));
                }
            }
        }

        /// <summary>
        /// Gets the URL where this version of this document can be found online
        /// </summary>
        public Uri ThisVersionUrl
        {
            get
            {
                // Since change to SKOS compatible formats the mappings are created on the fly, so there are no numbered versions any more
                if (this is EsdMapping) return this.LatestVersionUrl;

                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();
                return new Uri(nav.GetAttribute("HomeLocation", ""));
            }
        }

        /// <summary>
        /// Gets the version number of the XML document
        /// </summary>
        public float Version
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();

                string versionNumber = nav.GetAttribute("Version", String.Empty);
                if (!String.IsNullOrEmpty(versionNumber))
                {
                    return Single.Parse(versionNumber.Replace("Version ", String.Empty), CultureInfo.CreateSpecificCulture("en-GB"));
                }
                else return 0;
            }
        }


        #endregion // Properties encapsulating specific XML data

        #region Update XML from ESD website
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
            EsdXmlDocument latestXml = this.GetXmlFromWeb();

            // Version number is not consistently specified, so be prepared for errors
            float currentVersion = 0;
            float latestVersion = 0;
            try
            {
                currentVersion = this.Version;
            }
            catch (FormatException ex)
            {
                ex.Data.Add("Controlled list URL", this.ThisVersionUrl.ToString());
                throw;
            }
            try
            {
                latestVersion = latestXml.Version;
            }
            catch (FormatException ex)
            {
                ex.Data.Add("Controlled list URL", this.LatestVersionUrl.ToString());
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

                    // Update the live data too
                    this.LoadXmlDom(latestXml.OuterXml);
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
        /// <returns>The downloaded list, or null</returns>
        /// <exception cref="System.Security.SecurityException">Thrown if the CLR denies permission to access the Internet</exception>
        /// <exception cref="System.Net.WebException">Thrown if there was a problem accessing the Internet</exception>
        /// <exception cref="System.Xml.XmlException">Thrown is XML downloaded from web is not well formed</exception>
        /// <remarks>To use a proxy server, add its IP to the appSettings section of the application configuration file, in an element with a key named "ProxyServer". 
        /// It will use the current Windows credentials. To use different credentials, add extra elements with the following keys: "ProxyUser", "ProxyPassword" and "ProxyDomain".</remarks>
        public EsdXmlDocument GetXmlFromWeb()
        {
            WebPermission xmlWebPermission = new WebPermission(NetworkAccess.Connect, this.LatestVersionUrl.ToString());
            xmlWebPermission.Demand();

            WebRequest xmlRequest = XmlHttpRequest.Create(this.LatestVersionUrl);
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
                xmlRequest = XmlHttpRequest.Create(this.LatestVersionUrl);
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

            string currentVersion = this.Version.ToString(CultureInfo.CurrentCulture);
            string newName = String.Format(CultureInfo.InvariantCulture, "{0}-v{1}.backup{2}", Path.GetFileNameWithoutExtension(currentFile), currentVersion, Path.GetExtension(currentFile));
            string backupFolder = Path.GetDirectoryName(currentFile) + Path.DirectorySeparatorChar + "backup" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(backupFolder)) Directory.CreateDirectory(backupFolder);
            File.Copy(currentFile, backupFolder + newName, true);

            return true;
        }
        #endregion // Update XML from ESD website

    }
}
