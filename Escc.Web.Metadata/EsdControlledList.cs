using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.XPath;
using Escc.Web.Metadata.Properties;

namespace Escc.Web.Metadata
{
    /// <summary>
    /// A controlled list of terms from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
    /// </summary>
    public class EsdControlledList : EsdXmlDocument
    {

        #region Private fields to cache properties

        private bool hasNonPreferredTerms;
        private bool hasNonPreferredTermsSet;
        private bool hasRelatedTerms;
        private bool hasRelatedTermsSet;
        private IList<EsdMapping> mappings = new List<EsdMapping>();
        private bool skosCompatible = true; // assume up-to-date

        #endregion Private fields to cache properties

        #region Static factory methods
        /// <summary>
        /// Gets the XML for an ESD controlled list, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;Escc.Web.Metadata/ControlledListXml&gt; section of web.config</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdControlledList GetControlledList(string configKey)
        {
            return EsdControlledList.GetControlledList(configKey, false);
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list, cacheing it if possible
        /// </summary>
        /// <param name="configKey">Key for XML file name &lt;Escc.Web.Metadata/ControlledListXml&gt; section of web.config</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdControlledList GetControlledList(string configKey, bool withDom)
        {
            HttpContext ctx = HttpContext.Current;
            EsdControlledList xmlDoc = null;
            string cacheKey = String.Format(CultureInfo.InvariantCulture, "EsdXml_{0}", configKey);

            // Check for the document in the Application Data Cache
            if (ctx != null)
            {
                xmlDoc = (EsdControlledList)ctx.Cache.Get(cacheKey);
            }

            // Not in cache, so load XML 
            if (xmlDoc == null)
            {
                NameValueCollection config = ConfigurationManager.GetSection("Escc.Web.Metadata/ControlledListXml") as NameValueCollection;
                if (config == null) config = ConfigurationManager.GetSection("EsccWebTeam.Egms/ControlledListXml") as NameValueCollection;
                if (config == null) config = ConfigurationManager.GetSection("egmsXml") as NameValueCollection;
                if (config != null)
                {
                    if (String.IsNullOrEmpty(config[configKey]))
                    {
                        throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, Resources.ErrorConfigListNotFound, configKey));
                    }

                    xmlDoc = new EsdControlledList();
                    if (withDom)
                    {
                        xmlDoc.LoadDom(config[configKey]);
                    }
                    else
                    {
                        xmlDoc.LoadXPath(config[configKey]);
                    }
                }
                else
                {
                    throw new ConfigurationErrorsException(Resources.ErrorConfig);
                }

                // Cache me if you can
                if (ctx != null) ctx.Cache.Insert(cacheKey, xmlDoc, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));
            }

            // See whether this XML is in the new SKOS-compatible format or the old format. The old format used the Preferred attribute.
            xmlDoc.skosCompatible = (xmlDoc.SelectNodes("ns:Item[@Preferred]").Count == 0);

            return xmlDoc;
        }

        /// <summary>
        /// Gets the XML for an ESD controlled list, cacheing it if possible
        /// </summary>
        /// <param name="xmlFileUri">The URI of the XML file.</param>
        /// <param name="withDom">true to include DOM and XPath support; false for XPath only</param>
        /// <returns>Populated XML document, or null if not found</returns>
        public static EsdControlledList GetControlledList(Uri xmlFileUri, bool withDom)
        {
            HttpContext ctx = HttpContext.Current;
            EsdControlledList xmlDoc = null;
            string cacheKey = String.Format(CultureInfo.InvariantCulture, "EsdXml_{0}", Regex.Replace(xmlFileUri.ToString(), "[^A-Za-z]", String.Empty));

            // Check for the document in the Application Data Cache
            if (ctx != null)
            {
                xmlDoc = (EsdControlledList)ctx.Cache.Get(cacheKey);
            }

            // Not in cache, so load XML 
            if (xmlDoc == null)
            {
                xmlDoc = new EsdControlledList();
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

            // See whether this XML is in the new SKOS-compatible format or the old format. The old format used the Preferred attribute.
            xmlDoc.skosCompatible = (xmlDoc.SelectNodes("ns:Item[@Preferred]").Count == 0);

            return xmlDoc;
        }

        #endregion // Static factory methods

        #region Constructor
        /// <summary>
        /// Creates a controlled list of terms from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
        /// </summary>
        public EsdControlledList()
        {

        }


        #endregion // Constructor

        #region Properties encapsulating specific XML data
        /// <summary>
        /// Gets the date and time the version of the list was released
        /// </summary>
        public DateTime VersionDate
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();

                return DateTime.Parse(nav.GetAttribute("VersionDate", ""), CultureInfo.CreateSpecificCulture("en-GB"));
            }
        }

        /// <summary>
        /// Gets the abbreviated name of the controlled list
        /// </summary>
        public string AbbreviatedName
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();

                // Pre SKOS alignment the list acronym used to be in the ItemName property, but now that's a code so tidy it up as best as possible
                string abbreviatedName = Regex.Replace(nav.GetAttribute("ItemName", ""), "[0-9_]", String.Empty);
                if (abbreviatedName == abbreviatedName.ToLower(CultureInfo.CurrentCulture)) abbreviatedName = abbreviatedName.ToUpper(CultureInfo.CurrentCulture);
                return abbreviatedName;
            }
        }

        /// <summary>
        /// Gets the full name of the controlled list
        /// </summary>
        public string ListName
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNavigator nav = this.Xml.CreateNavigator();
                nav.MoveToFirstChild();
                return nav.GetAttribute("ListName", "");
            }
        }

        /// <summary>
        /// Gets the full name of the list, including abbreviation and version
        /// </summary>
        public string FullName
        {
            get
            {
                string full = this.ListName;
                if (!this.skosCompatible && this.AbbreviatedName != null && this.AbbreviatedName.Length > 0)
                {
                    full = String.Format(CultureInfo.CurrentCulture, "{0} ({1})", full, this.AbbreviatedName);
                }
                if (this.Version > 0) full = String.Format(CultureInfo.CurrentCulture, "{0} v{1}", full, this.Version.ToString(CultureInfo.CurrentCulture));
                return full;
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

                XPathNodeIterator it = this.SelectNodes("/ns:ControlledList/ns:Metadata/ns:Title");

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

                XPathNodeIterator it = this.SelectNodes("/ns:ControlledList/ns:Metadata/ns:Description");

                while (it.MoveNext())
                {
                    return it.Current.Value;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the date the list was published
        /// </summary>
        public DateTime DateIssued
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ControlledList/ns:Metadata/ns:Date.Issued");

                while (it.MoveNext())
                {
                    return DateTime.Parse(it.Current.Value, CultureInfo.CreateSpecificCulture("en-GB")); ;
                }

                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// Gets the date the list was last modified
        /// </summary>
        public DateTime DateModified
        {
            get
            {
                if (!this.XPathDataLoaded) throw new NotSupportedException(Resources.ErrorXmlNotLoaded);

                XPathNodeIterator it = this.SelectNodes("/ns:ControlledList/ns:Metadata/ns:Date.Modified");

                while (it.MoveNext())
                {
                    return DateTime.Parse(it.Current.Value, CultureInfo.CreateSpecificCulture("en-GB")); ;
                }

                return this.DateIssued;
            }
        }

        #endregion // Properties encapsulating specific XML data

        #region Get specific terms
        /// <summary>
        /// Gets a specific ESD metadata preferred term
        /// </summary>
        /// <param name="termId">The id of the term to get</param>
        /// <returns>The relevant term, or null if not found</returns>
        public EsdTerm GetTerm(string termId)
        {
            return this.GetTerm(termId, EsdPreferredState.Preferred);
        }


        /// <summary>
        /// Gets a specific ESD metadata term
        /// </summary>
        /// <param name="termId">The id of the term to get</param>
        /// <param name="preferredState">Specify whether the term must be preferred or non-preferred (for SKOS-compatible XML after published after June 2010, only preferred is relevant)</param>
        /// <returns>The relevant term, or null if not found</returns>
        public EsdTerm GetTerm(string termId, EsdPreferredState preferredState)
        {
            string xPath = String.Empty;
            switch (preferredState)
            {
                case EsdPreferredState.Preferred:
                    xPath = this.skosCompatible ? "ns:Item[@Id=\"{0}\"]" : "ns:Item[@Preferred='true' and @Id=\"{0}\"]";
                    break;
                case EsdPreferredState.NonPreferred:
                    if (this.skosCompatible) return null; // non-preferred terms no longer have their own id
                    xPath = "ns:Item[@Preferred='false' and @Id=\"{0}\"]";
                    break;
                case EsdPreferredState.Any: // for SKOS-compatible this effectively is the same as EsdPreferredState.Preferred
                    xPath = "ns:Item[@Id=\"{0}\"]";
                    break;
            }
            xPath = String.Format(CultureInfo.InvariantCulture, xPath, termId.Trim());
            XPathNodeIterator it = this.SelectNodes(xPath);

            if (it.Count != 1) return null;

            it.MoveNext();
            return this.GetEsdTermFromXml(it.Current);
        }

        /// <summary>
        /// Gets the preferred term for a specific ESD metadata concept
        /// </summary>
        /// <param name="conceptId">The concept id of the term to get</param>
        /// <returns>The relevant term, or null if not found</returns>
        public EsdTerm GetPreferredTerm(string conceptId)
        {
            string xpath = this.skosCompatible ? "ns:Item[@ConceptId=\"{0}\"]" : "ns:Item[@Preferred='true' and @ConceptId=\"{0}\"]";
            XPathNodeIterator it = this.SelectNodes(String.Format(CultureInfo.InvariantCulture, xpath, conceptId.Trim()));

            if (it.Count != 1) return null;

            it.MoveNext();
            return this.GetEsdTermFromXml(it.Current);
        }


        /// <summary>
        /// Gets the top level terms from an ESD controlled list
        /// </summary>
        public EsdTermCollection RootTerms
        {
            get
            {
                string xpath = this.skosCompatible ? "/ns:ControlledList/ns:Item[count(ns:BroaderItem)=0]" : "/ns:ControlledList/ns:Item[@Preferred='true' and count(ns:BroaderItem)=0]";
                XPathNodeIterator it = this.SelectNodes(xpath);
                EsdTermCollection terms = new EsdTermCollection();
                while (it.MoveNext())
                {
                    terms.Add(this.GetEsdTermFromXml(it.Current));
                }

                terms.Sort();
                return terms;
            }
        }


        #endregion // Get terms by partial or full name

        #region Get terms by partial or full name
        /// <summary>
        /// Gets an ESD metadata preferred term based on the exact name of the term
        /// </summary>
        /// <param name="termName">The term name to search for</param>
        /// <returns>A collection of terms containing a maximum of one term.</returns>
        /// <remarks>Matching is not case-sensitive.</remarks>
        public EsdTermCollection GetTerms(string termName)
        {
            return this.GetTerms(termName, true, EsdPreferredState.Preferred);
        }

        /// <summary>
        /// Gets ESD metadata preferred terms based on the name of the term
        /// </summary>
        /// <param name="termName">The term name to search for</param>
        /// <param name="exactMatch">true to match the whole name of a term; false to match any part of the term</param>
        /// <returns>A collection of matching terms</returns>
        /// <remarks>Matching is not case-sensitive.</remarks>
        public EsdTermCollection GetTerms(string termName, bool exactMatch)
        {
            return this.GetTerms(termName, exactMatch, EsdPreferredState.Preferred);
        }

        /// <summary>
        /// Gets ESD metadata terms based on the name of the term
        /// </summary>
        /// <param name="termName">The term name to search for</param>
        /// <param name="exactMatch">true to match the whole name of a term; false to match any part of the term</param>
        /// <param name="preferredState">Specify whether the term must be preferred or non-preferred</param>
        /// <returns>A collection of matching terms</returns>
        /// <remarks>Matching is not case-sensitive. For SKOS-compatible lists (those published after June 2010) the behaviour 
        /// of this method has changed. When a search matches a non-preferred term it used to return the non-preferred term
        /// as a top-level object alongside preferred terms. For SKOS compatible lists it now returns the preferred term which 
        /// goes with the non-preferred term.</remarks>
        public EsdTermCollection GetTerms(string termName, bool exactMatch, EsdPreferredState preferredState)
        {
            // double-quotes (") are used to delimit xpath strings in this method because it allows apostrophes in the string

            string xPath = null;
            if (exactMatch)
            {
                switch (preferredState)
                {
                    case EsdPreferredState.Preferred:
                        xPath = this.skosCompatible ? "ns:Item[ns:Name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]"
                            : "ns:Item[@Preferred='true' and ns:Name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]";
                        break;
                    case EsdPreferredState.NonPreferred:
                        xPath = this.skosCompatible ? "ns:Item[ns:AlternativeName[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]"
                    : "ns:Item[@Preferred='false' and ns:Name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]";
                        break;
                    case EsdPreferredState.Any:
                        xPath = this.skosCompatible ? "ns:Item[ns:Name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"] or ns:AlternativeName[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]"
                            : "ns:Item[ns:Name[translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')=\"{0}\"]]";
                        break;
                }
            }
            else
            {
                switch (preferredState)
                {
                    case EsdPreferredState.Preferred:
                        xPath = this.skosCompatible ? "ns:Item[ns:Name[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]"
                            : "ns:Item[@Preferred='true' and ns:Name[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]";
                        break;
                    case EsdPreferredState.NonPreferred:
                        xPath = this.skosCompatible ? "ns:Item[ns:AlternativeName[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]"
                            : "ns:Item[@Preferred='false' and ns:Name[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]";
                        break;
                    case EsdPreferredState.Any:
                        xPath = this.skosCompatible ? "ns:Item[ns:Name[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")] or ns:AlternativeName[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]"
                            : "ns:Item[ns:Name[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), \"{0}\")]]";
                        break;
                }
            }
            XPathNodeIterator it = this.SelectNodes(String.Format(CultureInfo.InvariantCulture, xPath, termName.Trim().ToLower(CultureInfo.CurrentCulture)));

            EsdTermCollection terms = new EsdTermCollection();
            while (it.MoveNext())
            {
                terms.Add(this.GetEsdTermFromXml(it.Current));
            }

            terms.Sort();
            return terms;
        }

        #endregion // Get terms by partial or full name

        #region Get terms by relationship with a specific term

        /// <summary>
        /// Gets whether the list contains any non-preferred terms
        /// </summary>
        /// <returns><c>true</c> if there's at least one non-preferred term; otherwise <c>false</c></returns>
        public bool HasNonPreferredTerms()
        {
            if (!this.hasNonPreferredTermsSet)
            {
                string xpath = this.skosCompatible ? "ns:Item/ns:AlternativeName" : "ns:Item[@Preferred='false']";
                XPathNodeIterator it = this.SelectNodes(xpath);
                this.hasNonPreferredTerms = (it.Count > 0);
                this.hasNonPreferredTermsSet = true;
            }

            return this.hasNonPreferredTerms;
        }

        /// <summary>
        /// Gets whether the list contains any related terms
        /// </summary>
        /// <returns><c>true</c> if there's at least one related term; otherwise <c>false</c></returns>
        public bool HasRelatedTerms()
        {
            if (!this.hasRelatedTermsSet)
            {
                XPathNodeIterator it = this.SelectNodes("ns:Item/ns:RelatedItem");
                this.hasRelatedTerms = (it.Count > 0);
                this.hasRelatedTermsSet = true;
            }

            return this.hasRelatedTerms;
        }

        /// <summary>
        /// Gets the non-preferred terms for the specified ESD metadata preferred term
        /// </summary>
        /// <param name="preferredTermId">The id of the preferred term for which to get non-preferred terms</param>
        /// <returns>A collection of non-preferred terms</returns>
        public EsdTermCollection GetNonPreferredTerms(string preferredTermId)
        {
            string xPath = this.skosCompatible ? "ns:Item[@Id='{0}']/ns:AlternativeName" : "ns:Item[@Preferred='false' and ns:UseItem[@Id='{0}']]";
            xPath = String.Format(CultureInfo.InvariantCulture, xPath, preferredTermId.Trim());
            XPathNodeIterator it = this.SelectNodes(xPath);

            EsdTermCollection terms = new EsdTermCollection();
            while (it.MoveNext())
            {
                EsdTerm term = this.GetEsdTermFromXml(it.Current);
                terms.Add(term);
            }

            terms.Sort();
            return terms;
        }

        /// <summary>
        /// Gets the ESD metadata preferred terms related to the specified term
        /// </summary>
        /// <param name="termId">The id of the term for which to get related terms</param>
        /// <returns>A collection of related terms</returns>
        public EsdTermCollection GetRelatedTerms(string termId)
        {
            return this.GetRelatedTerms(termId, null);
        }

        /// <summary>
        /// Gets the ESD metadata preferred terms related to the specified term
        /// </summary>
        /// <param name="termId">The id of the term for which to get related terms</param>
        /// <param name="termIdToExclude">The id of a term which should not be included even if it is related to the specified term</param>
        /// <returns>A collection of related terms</returns>
        public EsdTermCollection GetRelatedTerms(string termId, string termIdToExclude)
        {
            // get mappings to related terms
            string xPath;
            if (termIdToExclude != null && termIdToExclude.Length > 0)
            {
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:Item[@Id='{0}']/ns:RelatedItem[@Id!='{1}']", termId.Trim(), termIdToExclude.Trim());
            }
            else
            {
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:Item[@Id='{0}']/ns:RelatedItem", termId.Trim());
            }
            XPathNodeIterator it = this.SelectNodes(xPath);

            // get details of mapped terms
            EsdTermCollection terms = new EsdTermCollection();
            while (it.MoveNext())
            {
                EsdTerm term = this.GetTerm(it.Current.GetAttribute("Id", ""), EsdPreferredState.Preferred);
                if (term != null) terms.Add(term);
            }

            terms.Sort();
            return terms;
        }


        /// <summary>
        /// Gets the child terms of the specified ESD metadata term
        /// </summary>
        /// <param name="termId">The id of the term to get</param>
        /// <returns>A collection of child terms</returns>
        public EsdTermCollection GetChildTerms(string termId)
        {
            // get child terms
            string xPath = String.Format(CultureInfo.InvariantCulture, "ns:Item[ns:BroaderItem[@Id='{0}']]", termId.Trim());
            XPathNodeIterator it = this.SelectNodes(xPath);

            EsdTermCollection terms = new EsdTermCollection();
            while (it.MoveNext())
            {
                EsdTerm term = this.GetEsdTermFromXml(it.Current);
                if (term != null) terms.Add(term);
            }

            terms.Sort();
            return terms;
        }

        /// <summary>
        /// Gets the descendant terms of the specified ESD metadata term.
        /// </summary>
        /// <param name="termId">The id of the term whose descendants are required.</param>
        /// <returns>Descendant terms in a flat, non-hierarchical collection</returns>
        public EsdTermCollection GetDescendantTerms(string termId)
        {
            return this.GetDescendantTerms(termId, -1);
        }

        /// <summary>
        /// Gets the descendant terms of the specified ESD metadata term.
        /// </summary>
        /// <param name="termId">The id of the term whose descendants are required.</param>
        /// <param name="recurseLevels">How many levels down the tree to go. If <c>-1</c>, get all descendants.</param>
        /// <returns>Descendant terms in a flat, non-hierarchical collection</returns>
        public EsdTermCollection GetDescendantTerms(string termId, int recurseLevels)
        {
            if (recurseLevels == 0) return new EsdTermCollection(); // no point searching if results aren't wanted
            EsdTerm queryTerm = new EsdTerm();
            queryTerm.Id = termId;
            return GetDescendantTerms(queryTerm, recurseLevels, false);
        }

        /// <summary>
        /// Gets the descendant terms of the specified ESD metadata term.
        /// </summary>
        /// <param name="ancestorTerm">The ancestor term.</param>
        /// <returns>The original term with its ChildTerms property populated</returns>
        public EsdTerm GetDescendantTerms(EsdTerm ancestorTerm)
        {
            return GetDescendantTerms(ancestorTerm, -1);
        }

        /// <summary>
        /// Gets the descendant terms of the specified ESD metadata term.
        /// </summary>
        /// <param name="ancestorTerm">The ancestor term.</param>
        /// <param name="recurseLevels">How many levels down the tree to go. If <c>-1</c>, get all descendants.</param>
        /// <returns>The original term with its ChildTerms property populated</returns>
        public EsdTerm GetDescendantTerms(EsdTerm ancestorTerm, int recurseLevels)
        {
            if (recurseLevels == 0) return ancestorTerm; // no point searching if results aren't wanted
            EsdTermCollection ancestorInCollection = GetDescendantTerms(ancestorTerm, recurseLevels, true);
            return (ancestorInCollection.Count == 1) ? ancestorInCollection[0] : null;
        }

        /// <summary>
        /// Gets the descendant terms of the specified ESD metadata term.
        /// </summary>
        /// <param name="ancestorTerm">The ancestor term.</param>
        /// <param name="recurseLevels">How many levels down the tree to go. If <c>-1</c>, get all descendants.</param>
        /// <param name="updateAncestorTerm">if set to <c>true</c> updates the ancestor term and returns it in a 1-item collection; if <c>false returns a flat collection of all ancestor terms</c></param>
        private EsdTermCollection GetDescendantTerms(EsdTerm ancestorTerm, int recurseLevels, bool updateAncestorTerm)
        {
            EsdTermCollection descendantTerms = new EsdTermCollection();
            EsdTermCollection currentLevel = new EsdTermCollection();
            currentLevel.Add(ancestorTerm);
            EsdTermCollection nextLevel = new EsdTermCollection();
            while (recurseLevels > 0 || recurseLevels == -1)
            {
                foreach (EsdTerm currentTerm in currentLevel)
                {
                    EsdTermCollection childTerms = this.GetChildTerms(currentTerm.Id);

                    // add those child terms to the overall collection, and to the collection for querying the next level
                    foreach (EsdTerm childTerm in childTerms)
                    {
                        if (updateAncestorTerm)
                        {
                            currentTerm.ChildTerms.Add(childTerm);
                        }
                        else
                        {
                            descendantTerms.Add(childTerm);
                        }
                        nextLevel.Add(childTerm);
                    }
                }

                // If there are no more children, break out
                // This is essential for the "all levels" setting to prevent an infinite loop
                if (nextLevel.Count == 0) break;

                // otherwise step down to find children of these children
                currentLevel = nextLevel;
                nextLevel = new EsdTermCollection(); // don't use .Clear() because that clears currentLevel
                if (recurseLevels > 0) recurseLevels--;
            }

            if (updateAncestorTerm) descendantTerms.Add(ancestorTerm);
            return descendantTerms;
        }


        /// <summary>
        /// Gets the default parent ESD metadata term of the specified term
        /// </summary>
        /// <param name="termId">The id of the term for which to get the default parent term</param>
        /// <returns>A collection of terms containing a maximum of one term.</returns>
        public EsdTermCollection GetBroaderTerms(string termId)
        {
            return this.GetBroaderTerms(termId, true);
        }

        /// <summary>
        /// Gets the parent ESD metadata terms of the specified term
        /// </summary>
        /// <param name="termId">The id of the term for which to get parent terms</param>
        /// <param name="defaultOnly">ESD metadata lists are polyhierarchical - terms can appear in multiple positions, but before the SKOS alignment each had a single default position</param>
        /// <returns>A collection of broader terms. If <code>defaultOnly</code> is <code>true</code>, the collection will contain a maximum of one term.</returns>
        public EsdTermCollection GetBroaderTerms(string termId, bool defaultOnly)
        {
            // get mappings to broader terms
            string xPath;
            if (defaultOnly)
            {
                // account for GCL or all SKOS-compatible lists which don't have default attribute 
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:Item[@Id='{0}']/ns:BroaderItem[@Default='true' or count(@Default)=0]", termId.Trim());
            }
            else
            {
                xPath = String.Format(CultureInfo.InvariantCulture, "ns:Item[@Id='{0}']/ns:BroaderItem", termId.Trim());
            }
            XPathNodeIterator it = this.SelectNodes(xPath);

            // get details of broader terms
            EsdTermCollection terms = new EsdTermCollection();
            while (it.MoveNext())
            {
                EsdTerm term = this.GetTerm(it.Current.GetAttribute("Id", ""), EsdPreferredState.Preferred);
                if (term != null) terms.Add(term);
            }

            terms.Sort();
            return terms;
        }

        #endregion 		// Get terms by relationship with a specific term

        #region Build term object from XPathNavigator
        /// <summary>
        /// Builds data from an &lt;Item /&gt; node navigator into an EsdTerm
        /// </summary>
        /// <param name="itemNode">An &lt;Item /&gt; node navigator from an ESD XML file</param>
        /// <returns>An EsdTerm containing data from the &lt;Item /&gt; node</returns>
        private EsdTerm GetEsdTermFromXml(XPathNavigator itemNode)
        {
            CultureInfo nodeCulture = CultureInfo.CreateSpecificCulture(itemNode.XmlLang);

            EsdTerm term = new EsdTerm();
            term.ControlledList = this;

            // For any term in old format, or preferred terms in SKOS-compatible format
            if (itemNode.Name == "Item")
            {
                term.Id = itemNode.GetAttribute("Id", "");
                if (itemNode.GetAttribute("Preferred", "").Length > 0)
                {
                    term.Preferred = Boolean.Parse(itemNode.GetAttribute("Preferred", ""));
                }
                else if (this.skosCompatible)
                {
                    term.Preferred = true;
                }
                if (itemNode.GetAttribute("Obsolete", "").Length > 0) term.Obsolete = Boolean.Parse(itemNode.GetAttribute("Obsolete", ""));
                string addedInVersion = itemNode.GetAttribute("AddedInVersion", "");
                if (addedInVersion.Length > 0) term.AddedInVersion = Single.Parse(addedInVersion, nodeCulture); // value will be blank for any lists published after June 2010 when ESD stopped tracking this info
                string updatedInVersion = itemNode.GetAttribute("LastUpdatedInVersion", "");
                if (updatedInVersion.Length > 0) term.UpdatedInVersion = Single.Parse(updatedInVersion, nodeCulture); // value will be blank for any lists published after June 2010 when ESD stopped tracking this info
                if (itemNode.GetAttribute("AToZ", "").Length > 0) term.AtoZ = Boolean.Parse(itemNode.GetAttribute("AToZ", ""));
                if (itemNode.GetAttribute("Category", "").Length > 0) term.Category = Boolean.Parse(itemNode.GetAttribute("Category", ""));
                if (itemNode.GetAttribute("Type", "").Length > 0)
                {
                    try
                    {
                        term.EquivalentType = (EsdEquivalentType)Enum.Parse(typeof(EsdEquivalentType), itemNode.GetAttribute("Type", "").Replace(" ", ""), true);
                    }
                    catch (FormatException)
                    {
                        term.EquivalentType = EsdEquivalentType.NotSpecified;
                    }
                }

                // Get the term name. There may be different names for other languages, but we're
                // interested in the default name - ie: the one that matches the language of the whole node
                XPathNodeIterator termName = itemNode.SelectChildren("Name", itemNode.NamespaceURI);
                if (termName.Count > 0)
                {
                    while (termName.MoveNext())
                    {
                        if (termName.Current.XmlLang == nodeCulture.Name)
                        {
                            term.Text = termName.Current.Value;
                        }
                    }
                }
            }
            else
            {
                // For SKOS-compatible non-preferred terms
                term.Text = itemNode.InnerXml;
                itemNode.MoveToParent();
            }

            // Useful to return these properties of the preferred term for the non-preferred term too
            term.ConceptId = itemNode.GetAttribute("ConceptId", "");

            XPathNodeIterator it = itemNode.SelectChildren("ScopeNotes", itemNode.NamespaceURI);
            if (it.Count == 1)
            {
                it.MoveNext();
                term.ScopeNotes = it.Current.Value;
            }

            it = itemNode.SelectChildren("HistoryNotes", itemNode.NamespaceURI);
            if (it.Count == 1)
            {
                it.MoveNext();
                term.HistoryNotes = it.Current.Value;
            }

            return term;
        }
        #endregion // Build term object from XPathNavigator

        #region Relationships with other lists

        /// <summary>
        /// Mappings to and from other controlled lists
        /// </summary>
        public IList<EsdMapping> Mappings
        {
            get { return this.mappings; }
        }
        #endregion Relationships to other lists

    }
}
