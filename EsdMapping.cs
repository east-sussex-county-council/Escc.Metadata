using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.XPath;
using EsccWebTeam.Egms.Properties;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
    /// <summary>
    /// A mapping between two controlled lists of terms from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
    /// </summary>
    public class EsdMapping : EsdXmlDocument
    {

        #region Private fields

        private EsdControlledList toList;
        private EsdControlledList fromList;

        #endregion Private fields

        #region Constructor
        /// <summary>
        /// Creates a mapping between two controlled lists of terms from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
        /// </summary>
        public EsdMapping()
        {

        }
        #endregion // Constructor

        #region Static factory methods
        /// <summary>
        /// Gets the XML for an ESD list-to-list mapping, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;EsccWebTeam.Egms/ControlledListXml&gt; section of web.config</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdMapping GetMapping(string configKey)
        {
            return EsdMapping.GetMapping(configKey, false);
        }

        /// <summary>
        /// Gets the XML for an ESD list-to-list mapping, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;EsccWebTeam.Egms/ControlledListXml&gt; section of web.config</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdMapping GetMapping(string configKey, bool withDom)
        {
            HttpContext ctx = HttpContext.Current;
            EsdMapping xmlDoc = null;
            string cacheKey = String.Format(CultureInfo.InvariantCulture, "EsdXml_{0}", configKey);

            // Check for the document in the Application Data Cache
            if (ctx != null)
            {
                xmlDoc = (EsdMapping)ctx.Cache.Get(cacheKey);
            }

            // Not in cache, so load XML 
            if (xmlDoc == null)
            {
                NameValueCollection config = ConfigurationManager.GetSection("EsccWebTeam.Egms/ControlledListXml") as NameValueCollection;
                if (config == null) config = ConfigurationManager.GetSection("egmsXml") as NameValueCollection;
                if (config != null)
                {
                    xmlDoc = new EsdMapping();
                    if (withDom)
                    {
                        xmlDoc.LoadDom(config[configKey]);
                    }
                    else
                    {
                        xmlDoc.LoadXPath(config[configKey]);
                    }
                }

                // Cache me if you can
                if (ctx != null) ctx.Cache.Insert(cacheKey, xmlDoc, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));
            }

            return xmlDoc;
        }

        /// <summary>
        /// Gets the XML for an ESD list-to-list mapping, cacheing it if possible
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdMapping GetMapping(Uri xmlFileUri)
        {
            return EsdMapping.GetMapping(xmlFileUri, false);
        }

        /// <summary>
        /// Gets the XML for an ESD list-to-list mapping, cacheing it if possible
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdMapping GetMapping(Uri xmlFileUri, bool withDom)
        {
            HttpContext ctx = HttpContext.Current;
            EsdMapping xmlDoc = null;
            string cacheKey = String.Format(CultureInfo.InvariantCulture, "EsdXml_{0}", Regex.Replace(xmlFileUri.ToString(), "[^A-Za-z]", String.Empty));

            // Check for the document in the Application Data Cache
            if (ctx != null)
            {
                xmlDoc = (EsdMapping)ctx.Cache.Get(cacheKey);
            }

            // Not in cache, so load XML 
            if (xmlDoc == null)
            {
                xmlDoc = new EsdMapping();
                if (withDom)
                {
                    xmlDoc.LoadDom(xmlFileUri.ToString());
                }
                else
                {
                    xmlDoc.LoadXPath(xmlFileUri.ToString());
                }

                // Cache me if you can
                if (ctx != null) ctx.Cache.Insert(cacheKey, xmlDoc, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));
            }

            return xmlDoc;
        }

        #endregion // Static factory methods

        #region Properties encapsulating specific XML data

        /// <summary>
        /// Gets the URL for the controlled list this mapping starts from
        /// </summary>
        public Uri FromListUrl
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();
                return new Uri(nav.GetAttribute("FromResource", ""));
            }
        }

        /// <summary>
        /// Gets the URL for the controlled list this mapping goes to
        /// </summary>
        public Uri ToListUrl
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();
                return new Uri(nav.GetAttribute("ToResource", ""));
            }
        }

        /// <summary>
        /// Gets the alternative title for the document
        /// </summary>
        public string AlternativeTitle
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Title.Alternative");
                if (it.Count == 0) it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Title.AlternativeTitle");

                while (it.MoveNext())
                {
                    return it.Current.Value;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the title of the document
        /// </summary>
        public string Title
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Title");

                while (it.MoveNext())
                {
                    return it.Current.Value;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the description of the document
        /// </summary>
        public string Description
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Description");

                while (it.MoveNext())
                {
                    return it.Current.Value;
                }

                return String.Empty;
            }
        }


        /// <summary>
        /// Gets the date the mapping was published
        /// </summary>
        public DateTime DateIssued
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Date.Issued");

                while (it.MoveNext())
                {
                    return DateTime.Parse(it.Current.Value, CultureInfo.CreateSpecificCulture("en-GB")); ;
                }

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the date the mapping was last modified
        /// </summary>
        public DateTime DateModified
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ItemMappings/ns:Metadata/ns:Date.Issued");

                while (it.MoveNext())
                {
                    return DateTime.Parse(it.Current.Value, CultureInfo.CreateSpecificCulture("en-GB")); ;
                }

                return this.DateIssued;
            }
        }

        #endregion // Properties encapsulating specific XML data

        #region Relationships to lists

        /// <summary>
        /// Gets the controlled list this mapping goes to
        /// </summary>
        public EsdControlledList ToList
        {
            get { return this.toList; }
            set { this.toList = value; }
        }

        /// <summary>
        /// Gets the controlled list this mapping starts from
        /// </summary>
        public EsdControlledList FromList
        {
            get { return this.fromList; }
            set { this.fromList = value; }
        }

        /// <summary>
        /// Determines whether this mapping is from the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// 	<c>true</c> if this mapping is from the specified list; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMappingFrom(EsdControlledList list)
        {
            return MakeListUriComparable(list.ThisVersionUrl) == MakeListUriComparable(this.FromListUrl);
        }

        /// <summary>
        /// Determines whether this mapping is to the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// 	<c>true</c> if this mapping is to the specified list; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMappingTo(EsdControlledList list)
        {
            return MakeListUriComparable(list.ThisVersionUrl) == MakeListUriComparable(this.ToListUrl);
        }

        /// <summary>
        /// Determines whether this mapping is a mapping of the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// 	<c>true</c> if this mapping is a mapping of the specified list; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMappingOf(EsdControlledList list)
        {
            return (IsMappingFrom(list) || IsMappingTo(list));
        }

        /// <summary>
        /// Makes the list URIs as specified in list XML and mapping XML comparable.
        /// </summary>
        /// <param name="listUrl">The list URL.</param>
        /// <returns></returns>
        private Uri MakeListUriComparable(Uri listUrl)
        {
            List<string> segments = new List<string>(listUrl.Segments);
            segments.RemoveAt(segments.Count - 1); // Remove filename, which can be inconsistent
            if (Regex.IsMatch(segments[segments.Count - 1], @"^[/0-9.]$")) segments.RemoveAt(segments.Count - 1); // Remove version, so mapping to any version of list will match. Only applies to older URLs, hence numeric check.
            Uri comparableUri = new Uri(listUrl.Scheme + "://" + listUrl.Host + String.Join("", segments.ToArray()));
            return comparableUri;
        }

        #endregion Other properties

        #region Get terms using the mapping data
        /// <summary>
        /// Gets the ESD metadata terms mapped to the specified terms in another ESD controlled list
        /// </summary>
        /// <param name="esdListXml">The XML file of the ESD controlled list containing the mapped terms</param>
        /// <param name="termsToMap">The terms for which to get mapped terms</param>
        /// <param name="mapForwards">If false, map from list B to list A, rather than list A to list B</param>
        /// <returns>A collection of mapped terms</returns>
        public EsdTermCollection GetMappedTerms(EsdControlledList esdListXml, EsdTermCollection termsToMap, bool mapForwards)
        {
            EsdTermCollection mappedTerms = new EsdTermCollection();

            foreach (EsdTerm termToMap in termsToMap)
            {
                EsdTermCollection termsFound = this.GetMappedTerms(esdListXml, termToMap.Id, mapForwards);
                foreach (EsdTerm foundTerm in termsFound)
                {
                    if (!mappedTerms.Contains(foundTerm)) mappedTerms.Add(foundTerm);
                }
            }

            return mappedTerms;
        }

        /// <summary>
        /// Gets the ESD metadata terms mapped to the specified terms in another ESD controlled list
        /// </summary>
        /// <param name="esdListXml">The XML file of the ESD controlled list containing the mapped terms</param>
        /// <param name="termsToMap">The terms for which to get mapped terms</param>
        /// <returns>A collection of mapped terms</returns>
        public EsdTermCollection GetMappedTerms(EsdControlledList esdListXml, EsdTermCollection termsToMap)
        {
            return this.GetMappedTerms(esdListXml, termsToMap, true);
        }

        /// <summary>
        /// Gets the ESD metadata terms mapped to the specified term in another ESD controlled list
        /// </summary>
        /// <param name="esdListXml">The XML file of the ESD controlled list containing the mapped terms</param>
        /// <param name="termId">The id of the term for which to get mapped terms</param>
        /// <returns>A collection of mapped terms</returns>
        public EsdTermCollection GetMappedTerms(EsdControlledList esdListXml, string termId)
        {
            return this.GetMappedTerms(esdListXml, termId, true);
        }

        /// <summary>
        /// Gets the ESD metadata terms mapped to the specified term in another ESD controlled list
        /// </summary>
        /// <param name="esdListXml">The XML file of the ESD controlled list containing the mapped terms</param>
        /// <param name="termId">The id of the term for which to get mapped terms</param>
        /// <param name="mapForwards">If false, map from list B to list A, rather than list A to list B</param>
        /// <returns>A collection of mapped terms</returns>
        public EsdTermCollection GetMappedTerms(EsdControlledList esdListXml, string termId, bool mapForwards)
        {
            return this.GetMappedTerms(esdListXml, termId, mapForwards, null);
        }

        /// <summary>
        /// Gets the ESD metadata terms mapped to the specified term in another ESD controlled list
        /// </summary>
        /// <param name="esdListXml">The XML file of the ESD controlled list containing the mapped terms</param>
        /// <param name="termId">The id of the term for which to get mapped terms</param>
        /// <param name="mapForwards">If false, map from list B to list A, rather than list A to list B</param>
        /// <param name="termIdToExclude">The id of a term which should not be included even if it is mapped to the specified term</param>
        /// <returns>A collection of mapped terms</returns>
        public EsdTermCollection GetMappedTerms(EsdControlledList esdListXml, string termId, bool mapForwards, string termIdToExclude)
        {
            if (termId == null) return new EsdTermCollection();

            // get mappings
            string xPath;
            if (mapForwards)
            {
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:ItemMapping[ns:From/ns:Item[@Id='{0}']]", termId.Trim());
            }
            else
            {
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:ItemMapping[ns:To/ns:Item[@Id='{0}']]", termId.Trim());
            }
            XPathNodeIterator it = this.SelectNodes(xPath);

            // get details of mapped terms
            EsdTermCollection terms = new EsdTermCollection();
            if (mapForwards)
            {
                if (termIdToExclude != null && termIdToExclude.Length > 0)
                {
                    xPath = String.Format(CultureInfo.InvariantCulture, "ns:To/ns:Item[@Id!='{0}']", termIdToExclude);
                }
                else
                {
                    xPath = "ns:To/ns:Item";
                }
            }
            else
            {
                if (termIdToExclude != null && termIdToExclude.Length > 0)
                {
                    xPath = String.Format(CultureInfo.InvariantCulture, "ns:From/ns:Item[@Id!='{0}']", termIdToExclude);
                }
                else
                {
                    xPath = "ns:From/ns:Item";
                }
            }
            while (it.MoveNext())
            {
                XPathNodeIterator mappingNode = this.SelectNodes(xPath, it.Current);

                if (mappingNode.Count == 1)
                {
                    mappingNode.MoveNext();
                    EsdTerm term = esdListXml.GetTerm(mappingNode.Current.GetAttribute("Id", ""), EsdPreferredState.Preferred);
                    if (term != null) terms.Add(term);
                }

            }

            terms.Sort();
            return terms;
        }

        #endregion // Get terms using the mapping data
    }
}
