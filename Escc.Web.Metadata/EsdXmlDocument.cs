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
using Escc.Web.Metadata.Properties;

namespace Escc.Web.Metadata
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
        /// <param name="configKey">Key for XML file name &lt;Escc.Web.Metadata/ControlledListXml&gt; section of web.config</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdXmlDocument GetEsdDocument(string configKey)
        {
            return EsdXmlDocument.GetEsdDocument(configKey, false);
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list or mapping, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;Escc.Web.Metadata/ControlledListXml&gt; section of web.config</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdXmlDocument GetEsdDocument(string configKey, bool withDom)
        {
            // TODO: Ideally this would check the format of the file to decide, but as a quick fix
            // just check whether the key includes the word "mapping"

            NameValueCollection config = ConfigurationManager.GetSection("Escc.Web.Metadata/ControlledListXml") as NameValueCollection;
            if (config == null) config = ConfigurationManager.GetSection("EsccWebTeam.Egms/ControlledListXml") as NameValueCollection;
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

    }
}
