using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Escc.AddressAndPersonalDetails;
using EsccWebTeam.Data.Web;
using EsccWebTeam.Egms;

namespace EsccWebTeam.Egms
{
    /// <summary>
    /// Control to add e-GMS and other web metadata into the &lt;head&gt;&lt;/head&gt; of a web page
    /// </summary>
    /// <remarks>
    /// <para>This class is a partial implementation of the E-Government Metadata Standard (e-GMS) v3.0 for Web Sites, together with some additional elements commonly used on websites.</para>
    /// <para>The e-GMS v3.0 for Web Sites standard can be found at <a href="http://www.govtalk.gov.uk/schemasstandards/metadata_document.asp?docnum=806">http://www.govtalk.gov.uk/schemasstandards/metadata_document.asp?docnum=806</a>.</para>
    /// <para>Advice on applying e-GMS in local authorities can be found at <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
    /// <para>Controlled vocabularies referenced by several properties can be found at:</para>
    /// <list type="table">
    /// <listheader>
    ///		<term>Controlled vocabulary</term>
    ///		<description>URL for details</description>
    /// </listheader>
    /// <item>
    ///		<term>Integrated Public Sector Vocabulary (IPSV)</term>
    ///		<description><a href="http://www.esd.org.uk/standards/IPSV/">http://www.esd.org.uk/standards/IPSV/</a></description>
    ///	</item>
    /// <item>
    ///		<term>Local Government Interaction List (LGIL)</term>
    ///		<description><a href="http://www.esd.org.uk/standards/lgil/">http://www.esd.org.uk/standards/lgil/</a></description>
    ///	</item>
    /// <item>
    ///		<term>Local Government Service List (LGSL), also known as the PID List. A subset of the Public Sector Services List (PSSL).</term>
    ///		<description><a href="http://www.esd.org.uk/standards/lgsl/">http://www.esd.org.uk/standards/lgsl/</a></description>
    ///	</item>
    /// <item>
    ///		 <term>Local Government Type List (LGTL) </term>
    ///		<description><a href="http://www.esd.org.uk/standards/lgtl/">http://www.esd.org.uk/standards/lgtl/</a></description>
    ///	</item>
    /// <item>
    ///     <term>Education and Skills Thesaurus</term>
    ///     <description><a href="http://www.multites.co.uk/dfes/">http://www.multites.co.uk/dfes/</a></description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>The first example shows how to reference this control on an ASP.net web page. Note that some properties can be set using
    /// a template as well as an attribute. A template allows other controls to output the metadata value. If both attributes and templates
    /// are used to set the same metadata, the attribute will be ignored.</para>
    /// <code>
    ///		&lt;%@ Register TagPrefix=&quot;Egms&quot; Namespace=&quot;eastsussexgovuk.webservices.EgmsWebMetadata&quot; Assembly=&quot;EsccWebTeam.Egms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f&quot; %&gt;
    ///		
    ///		&lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
    ///		&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot; dir=&quot;ltr&quot; lang=&quot;en&quot; xml:lang=&quot;en&quot;&gt;
    ///			&lt;head&gt;
    ///
    ///				&lt;Egms:MetadataControl id=&quot;egms&quot; runat=&quot;server&quot; 
    ///					Title=&quot;This is an example&quot; 
    ///					PreFetchUrl=&quot;http://www.example.com/&quot;
    ///					&gt;
    ///					&lt;TitleTemplate&gt;This is an example&lt;/TitleTemplate&gt;
    ///				&lt;/Egms:MetadataControl&gt;
    ///					
    ///			&lt;/head&gt;
    ///		&lt;/html&gt;
    /// </code>
    /// <para>The second example shows all the possible attributes of the <b>EgmsWebMetadata</b> element which can be used in <b>web.config</b>. 
    /// A real example would use only a few of these. It is never appropriate to use all of them. For example, <b>dateAutoRemove</b> and 
    /// <b>dateReview</b> are mutually exclusive: the first indicates that content will be removed without review, the second that it will be reviewed.</para>
    /// <para>Other properties are available to set only on the page, as they are only appropriate for single pages rather than groups of pages</para>
    /// <code>
    ///		&lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;EsccWebTeam.Egms&quot;&gt;
    ///			    &lt;section name=&quot;EgmsWebMetadata&quot; type=&quot;EsccWebTeam.Egms.EgmsWebMetadataSectionHandler,EsccWebTeam.Egms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f&quot; /&gt;
    ///         &lt;/sectionGroup&gt;
    ///		&lt;/configSections&gt;
    ///		
    /// 	&lt;EsccWebTeam.Egms&gt;
    /// 	    &lt;EgmsWebMetadata
    ///			    errorMode=&quot;Off&quot;
    ///			    creator=&quot;Service Team, East Sussex County Council, Tel: 01273 481000&quot;
    ///			    contributor=&quot;Web Content Editor, Web Team, East Sussex County Council, Tel: 01273 481742&quot;
    ///			    custodian=&quot;ICT Services, East Sussex County Council, Tel: 01273 481234&quot;
    ///			    language=&quot;eng&quot;
    ///			    person=&quot;Councillor Smith&quot;
    ///			    publisher=&quot;East Sussex County Council&quot;
    ///			    replacesUrl=&quot;http://example.org/thisdocument/2004/&quot;
    ///			    isReplacedByUrl=&quot;http://example.org/thisdocument/2006/&quot;
    ///             isPartOfUrl=&quot;http://example.org/parentdocument/&quot;
    ///			    spatialCoverage=&quot;Eastbourne, East Sussex, UK&quot;
    ///			    temporalCoverage=&quot;2004/05&quot;
    ///			    type=&quot;StaffDirectory&quot;
    ///		    	dateCreated=&quot;2005-02-28&quot;
    ///			    dateIssued=&quot;2005-03-10&quot;
    ///		    	dateModified=&quot;2005-03-25&quot;
    ///		    	dateNextVersionDue=&quot;2006-01-01&quot;
    ///		    	dateAutoRemove=&quot;2005-12-31&quot;
    /// 		    dateReview=&quot;2005-12-20&quot;
    /// 	       	ipsvPreferredTerms=&quot;An IPSV preferred term; another IPSV preferred term&quot;
    /// 	    	ipsvNonPreferredTerms=&quot;An IPSV non-preferred term; another IPSV non-preferred term&quot;
    /// 		    keywords=&quot;technical; documentation; example&quot;
    /// 	       	educationAndSkillsPreferredTerms=&quot;An Education and Skills Thesaurus preferred term; another Education and Skills Thesaurus preferred term&quot;
    /// 	       	educationAndSkillsNonPreferredTerms=&quot;An Education and Skills Thesaurus non-preferred term; another Education and Skills Thesaurus non-preferred term&quot;
    /// 	    	foiExempt=&quot;true&quot;
    ///             lgslNumbers=&quot;123;456&quot;
    /// 	      	lgilType=&quot;Providing information&quot;
    /// 	    	audience=&quot;Non-LGAL audience;Another non-LGAL audience&quot;
    /// 	    	wcagLevel=&quot;AA&quot;
    /// 	    	copyrightUrl=&quot;/copyright/&quot;
    /// 	    	helpUrl=&quot;/help/&quot;
    /// 	    	searchUrl=&quot;/search/&quot;
    ///     		homeUrl=&quot;/home/&quot;
    /// 	    	helpToolTip=&quot;Help with using this site&quot;
    ///     		searchToolTip=&quot;Search this site&quot;
    /// 	    	homeToolTip=&quot;Go to our home page&quot;
    ///  	    	iconUrl=&quot;/favicon.ico&quot;
    ///  	    	hasTouchIcon=&quot;true&quot;
    ///     		prefetchUrl=&quot;http://example.org/somepage/nextpage.aspx&quot;
    /// 	    	openSearchUrl=&quot;http://example.org/somepage/search.xml&quot;
    ///     		openSearchTitle=&quot;Search this site&quot;
    ///     		twitterCardType=&quot;summary&quot;
    ///     		twitterAccount=&quot;@eastsussexcc&quot;
    /// 		/&gt;
    /// 	&lt;/EsccWebTeam.Egms&gt;
    /// </code>
    /// </example>
    [DefaultProperty("Title"),
    ToolboxData("<{0}:EgmsWebMetadataControl runat=server></{0}:EgmsWebMetadataControl>")]
    [ParseChildren(ChildrenAsProperties = true)]
    public class MetadataControl : System.Web.UI.WebControls.WebControl, INamingContainer, EsccWebTeam.Egms.IMetadata
    {
        #region Private fields
        private WcagConformance wcagLevel = WcagConformance.Unknown;
        private string title = String.Empty;
        private string titlePattern = String.Empty;
        private ITemplate titleTemplate;
        private string description = String.Empty;
        private string keywords = String.Empty;
        private string spatialCoverage = String.Empty;
        private string postcodes = String.Empty;
        private string creator = String.Empty;
        private string[] publishers;
        private string language = String.Empty;
        private string format = String.Empty;
        private string lgtlType = String.Empty;
        private string[] ipsvPreferredTerms;
        private string[] ipsvNonPreferredTerms;
        private string[] educationAndSkillsPreferredTerms;
        private string[] educationAndSkillsNonPreferredTerms;
        private string audience = String.Empty;
        private string custodian = String.Empty;
        private string contributor = String.Empty;
        private bool isInSearch = true;
        private string tagEnd = " />";
        private EgmsWebMetadataConfig config;
        private string systemId = String.Empty;
        private string filePlanId = String.Empty;
        private string dateCreated = String.Empty;
        private string dateModified = String.Empty;
        private string dateIssued = String.Empty;
        private string dateNextVersionDue = String.Empty;
        private string dateAutoRemove = String.Empty;
        private string dateReview = String.Empty;
        private string isReplacedByUrl = String.Empty;
        private string replacesUrl = String.Empty;
        private string person = String.Empty;
        private string[] lgslNumbers;
        private string lgilType = String.Empty;
        private string homeUrl = String.Empty;
        private string searchUrl = String.Empty;
        private string helpUrl = String.Empty;
        private string copyrightUrl = String.Empty;
        private string homeToolTip = String.Empty;
        private string searchToolTip = String.Empty;
        private string helpToolTip = String.Empty;
        private string temporalCoverage = String.Empty;
        private string preFetchUrl = String.Empty;
        private bool metadataRequired = true;
        private string iconUrl;
        private IList<FeedUrl> feeds = new FeedUrlCollection();
        private IList<OpenSearchUrl> openSearchUrls = new OpenSearchUrlCollection();
        private Control titleControl;
        private bool? foiExempt;
        private string isPartOfUrl = String.Empty;
        private IList<Uri> references = new Collection<Uri>();
        private IList<Uri> requires = new Collection<Uri>();
        private Int32 easting = default(Int32);
        private Int32 northing = default(Int32);
        private bool isContactDoc = default(bool);
        #endregion // Private fields


        #region Mandatory e-GMS element propeties

        /// <summary>
        /// Gets or sets level of conformance to the Web Content Accessibility Guidelines (WCAG) 1.0
        /// </summary>
        /// <remarks>
        /// <para>Possible values are "None", "A", "AA" and "AAA". All UK government web pages are expected to meet WCAG 1.0 Level "AA".</para>
        /// <para>WCAG 1.0 can be found at <a href="http://www.w3.org/TR/1999/WAI-WEBCONTENT-19990505/">http://www.w3.org/TR/1999/WAI-WEBCONTENT-19990505/</a>.</para>
        /// <para>Stating the level of accessibility is mandatory in e-GMS v3.0.</para>
        /// </remarks>
        public WcagConformance WcagLevel
        {
            get
            {
                return this.wcagLevel;
            }
            set
            {
                this.wcagLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the web page
        /// </summary>
        /// <remarks>
        /// <para>Title is mandatory in e-GMS v3.1.</para>
        /// <para>This property cannot be set in web.config. It must be set on each individual page, because it should be different for each individual page.</para>
        /// <para>If the current web page has a &lt;head runat=&quot;server&quot;> tag, this property will get and set the value of Page.Title.</para>
        /// </remarks>
        public string Title
        {
            get
            {
                string bestTitle = "";
                if (this.Page != null && this.Page.Header != null && this.Page.Title != null && this.Page.Title.Trim().Length > 0) bestTitle = this.Page.Title;
                else if (this.title != null) bestTitle = this.title;
                return bestTitle;
            }
            set
            {
                this.title = value;
                if (this.Page != null && this.Page.Header != null) this.Page.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets a pattern to be used to display the title, where the title replaces {0}
        /// </summary>
        /// <example>Standard text before the title {0} Standard text after the title</example>
        public string TitlePattern
        {
            get { return this.titlePattern; }
            set { this.titlePattern = (value == null) ? String.Empty : value; }
        }

        /// <summary>
        /// Gets or sets the language the title is written in.
        /// </summary>
        /// <value>The title language.</value>
        /// <remarks>For pages which use the built-in <seealso cref="System.Web.UI.HtmlControls.HtmlTitle"/> control, the only way to add language attributes seems to be to override the page render method</remarks>
        public string TitleLanguage { get; set; }

        /// <summary>
        /// Gets or sets the title of the web page using controls in a template
        /// </summary>
        /// <remarks>
        /// <para>Title is mandatory in e-GMS v3.1.</para>
        /// <para>This property cannot be set in web.config. It must be set on each individual page, because it should be different for each individual page.</para>
        /// </remarks>
        [TemplateContainer(typeof(MetadataContainer))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)] // This makes it validate in Visual Studio (allegedly)
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] // This makes it validate in Visual Studio (allegedly)
        public ITemplate TitleTemplate
        {
            get { return this.titleTemplate; }
            set { this.titleTemplate = value; }
        }


        /// <summary>
        /// Gets or sets the date the document was published in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued (this property) is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateIssued
        {
            get
            {
                return this.dateIssued;
            }
            set
            {
                this.dateIssued = value;
            }
        }

        /// <summary>
        /// Gets or sets the date the document was modified in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateModified
        {
            get
            {
                return this.dateModified;
            }
            set
            {
                this.dateModified = value;
            }
        }

        /// <summary>
        /// Gets or sets the date the document was created in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateCreated
        {
            get
            {
                return this.dateCreated;
            }
            set
            {
                this.dateCreated = value;
            }
        }


        /// <summary>
        /// Gets or sets a semi-colon separated list of publishers - the entities who need to give permission should someone want to republish the data
        /// </summary>
        /// <remarks>
        /// <para>Mandatory for web pages in e-GMS v3.0. Can usually be set to "East Sussex County Council".</para>
        /// <para>This property is called "Publisher" rather than "Publishers" for backwards compatibility - it used only to accept one publisher.</para>
        /// </remarks>
        public string Publisher
        {
            get
            {
                return (this.publishers != null) ? String.Join(";", this.publishers) : String.Empty;
            }
            set
            {
                this.publishers = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }

        /// <summary>
        /// Gets or sets the entity primarily responsible for the content of the web page
        /// </summary>
        /// <remarks>
        /// <para>Mandatory in e-GMS v3.0.</para>
        /// <para>Job titles should be used rather than names because they're less likely to change. 
        /// If names are used, Dublin Core recommends the "Lastname, firstname" format.</para>
        /// <para>In addition, give a telephone number or a generic email address. Do not use personal email addresses.</para>
        /// </remarks>
        public string Creator
        {
            get
            {
                return this.creator;
            }
            set
            {
                this.creator = value;
            }
        }

        /// <summary>
        /// Gets the default entity primarily responsible for the content of web pages
        /// </summary>
        /// <remarks>
        /// This property can be used when creating a derived control for a particular website. It enables a default creator 
        /// which can be overriden at web.config or page level.
        /// </remarks>
        protected virtual string DefaultCreator
        {
            get
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Integrated Public Sector Vocabulary (IPSV) preferred terms
        /// </summary>
        /// <remarks>
        /// <para>The use of IPSV preferred terms is mandatory in e-GMS 3.1.</para>
        /// <para>Always use the full version of the IPSV to find the relevant term(s). (There is also an abridged version, which is not suitable for local government.)</para>
        /// <para>Choose the term that is most relevant to the content from the complete hierarchy list. You can include more than one term if relevant. 
        /// You don’t need to include all categories as you drill down – just the one that best describes the content on the page.</para>
        /// </remarks>
        public string IpsvPreferredTerms
        {
            get
            {
                return (this.ipsvPreferredTerms != null) ? String.Join(";", this.ipsvPreferredTerms) : String.Empty;
            }
            set
            {
                this.ipsvPreferredTerms = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Integrated Public Sector Vocabulary (IPSV) non-preferred terms
        /// </summary>
        /// <remarks>The use of IPSV preferred terms is mandatory in e-GMS 3.1. However, it is not essential to include the non-preferred terms.</remarks>
        public string IpsvNonPreferredTerms
        {
            get
            {
                return (this.ipsvNonPreferredTerms != null) ? String.Join(";", this.ipsvNonPreferredTerms) : String.Empty;
            }
            set
            {
                this.ipsvNonPreferredTerms = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Education and Skills Thesaurus preferred terms
        /// </summary>
        /// <remarks>
        /// <para>The use of the Education and Skills Thesaurus is optional in e-GMS 3.1.</para>
        /// </remarks>
        public string EducationAndSkillsPreferredTerms
        {
            get
            {
                return (this.educationAndSkillsPreferredTerms != null) ? String.Join(";", this.educationAndSkillsPreferredTerms) : String.Empty;
            }
            set
            {
                this.educationAndSkillsPreferredTerms = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Education and Skills Thesaurus non-preferred terms
        /// </summary>
        /// <remarks>The use of the Education and Skills Thesaurus is optional in e-GMS 3.1.</remarks>
        public string EducationAndSkillsNonPreferredTerms
        {
            get
            {
                return (this.educationAndSkillsNonPreferredTerms != null) ? String.Join(";", this.educationAndSkillsNonPreferredTerms) : String.Empty;
            }
            set
            {
                this.educationAndSkillsNonPreferredTerms = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }

        /// <summary>
        /// Gets or sets an interaction type from the Local Government Interaction Type List (LGIL)
        /// </summary>
        /// <remarks>Most ordinary web pages should use the "Providing information" type. Some web applications will use "Applications for service".</remarks>
        public string LgilType
        {
            get
            {
                return this.lgilType;
            }
            set
            {
                this.lgilType = value;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of numeric service identifiers from the Local Government Service List (LGSL). Service names can optionally be included following service numbers, also separated by a semi-colon.
        /// </summary>
        /// <remarks>
        /// <para>LGSL numbers are required. The names of the services are optional extras, which can be included purely to make the data more readable.</para>
        /// <para>The LGSL is known on the ESD toolkit site as the PID List. It is a subset of the Public Sector Services List (PSSL).</para>
        /// <example>1; 2; 3</example>
        /// <example>1; Service one; 2; Service two; 3; Service three</example>
        /// <example>1; Service one; 2; 3; Service three</example>
        /// </remarks>
        public string LgslNumbers
        {
            get
            {
                return (this.lgslNumbers != null) ? String.Join(";", this.lgslNumbers) : String.Empty;
            }
            set
            {
                this.lgslNumbers = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }


        /// <summary>
        /// Gets or sets the unique identifier of the content in an electronic system - eg a database id.
        /// </summary>
        public string SystemId
        {
            get
            {
                return this.systemId;
            }
            set
            {
                this.systemId = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference derived from the fileplan. This is a culmination of information inherited from higher levels of aggregation in the fileplan.
        /// </summary>
        public string FilePlanId
        {
            get { return this.filePlanId; }
            set { this.filePlanId = value; }
        }


        #endregion // Mandatory e-GMS elements

        #region Recommended e-GMS element properties

        /// <summary>
        /// Gets or sets the language of the document using an ISO 639 language code
        /// </summary>
        /// <remarks>Recommended in e-GMS v3.0. If no language is specified, "eng" is used.</remarks>
        public string Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
            }
        }


        /// <summary>
        /// Gets or sets the geographical area covered by the web page
        /// </summary>
        /// <remarks>Recommended in e-GMS v3.0. The coverage should be unique enough to distinguish the 
        /// area from any other similiarly named area in the world. If no area is specified, "East Sussex, UK" will be used.</remarks>
        /// <example>Lewes, East Sussex, UK</example>
        public string SpatialCoverage
        {
            get
            {
                return this.spatialCoverage;
            }
            set
            {
                this.spatialCoverage = value;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Royal Mail postcodes which identify the geographical area(s) covered by the web page
        /// </summary>
        /// <remarks>Used as a value for spatial coverge, which is recommended in e-GMS 3.1.</remarks>
        /// <example>BN7 1UE; BN9 9TT</example>
        public string Postcodes
        {
            set { this.postcodes = value; }
            get { return this.postcodes; }
        }

        /// <summary>
        /// Gets or sets the time period the web page is about
        /// </summary>
        /// <remarks>There is no specific format, so "Tuesdays" is just as valid as "2001-2002".</remarks>
        public string TemporalCoverage
        {
            get
            {
                return this.temporalCoverage;
            }
            set
            {
                this.temporalCoverage = value;
            }
        }


        #endregion // Recommended e-GMS element properties

        #region Optional e-GMS element properties

        /// <summary>
        /// Gets or sets the type of content on the page - eg maps, minutes, graphs etc - as a value from the Local Government Type List (LGTL)
        /// </summary>
        /// <remarks>
        /// <para>Optional in e-GMS v3.0.</para>
        /// <para>The LGTL is not a mature standard, so you will often find there is no appropriate type. In this case, simply leave the property blank.</para>
        /// </remarks>
        public string LgtlType
        {
            get
            {
                return this.lgtlType;
            }
            set
            {
                this.lgtlType = value;
            }
        }

        /// <summary>
        /// Gets or sets the MIME type of the web page
        /// </summary>
        /// <remarks><para>Optional in e-GMS 3. If no value is specified, "text/html" will be used since it will always be correct for a web page 
        /// as long as Internet Explorer 6 or below is in common use.</para>
        /// <para>XHTML 1.0 web pages can use (and XHTML 1.1 pages <b>must</b> use) "application/xhtml+xml", but Internet Explorer 6 doesn't recognise it.</para>
        /// </remarks>
        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of additional keywords for the web page - those not taken from a controlled vocabulary
        /// </summary>
        /// <remarks>Optional in e-GMS v3.0.</remarks>
        public string Keywords
        {
            get
            {
                return this.keywords;
            }
            set
            {
                this.keywords = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of the web page
        /// </summary>
        /// <remarks>
        /// <para>Optional in e-GMS v3.0, but it should be set since this is the description which is displayed in search result listings.</para>
        /// <para>Try not to repeat information that could be held in another element, eg title, coverage or subject. The description should 
        /// be short but meaningful (under 30 words). It could include the approach to the subject, the reason it’s produced, groups and 
        /// organisations referred to, events covered, key outcomes, broad policy areas etc.</para>
        /// </remarks>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets the person who the web page is about
        /// </summary>
        /// <example>Councillor John Smith</example>
        public string Person
        {
            get
            {
                return this.person;
            }
            set
            {
                this.person = value;
            }
        }

        /// <summary>
        /// Gets or sets the person the page is about.
        /// </summary>
        /// <value>The person the page is about.</value>
        public Person PersonAbout { get; set; }

        /// <summary>
        /// Gets or sets an identifier of a resource which this web page replaces
        /// </summary>
        /// <remarks>This might be used where an archive of older versions of a document is available, perhaps for a document which is updated annually. 
        /// The property should be set to the URL of the older document.</remarks>
        public string ReplacesUrl
        {
            get
            {
                return this.replacesUrl;
            }
            set
            {
                this.replacesUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets an identifier of a resource which replaced this web page 
        /// </summary>
        /// <remarks>This might be used where an archive of older versions of a document is available, perhaps for a document which is updated annually. 
        /// The property should be set on the archived document, pointing to the URL of the newer document.</remarks>
        public string IsReplacedByUrl
        {
            get
            {
                return this.isReplacedByUrl;
            }
            set
            {
                this.isReplacedByUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the resource of which the current resource is a part
        /// </summary>
        /// <value>The URL of the referenced resource.</value>
        public string IsPartOfUrl
        {
            get { return this.isPartOfUrl; }
            set { this.isPartOfUrl = value; }
        }

        /// <summary>
        /// Gets resources referenced, cited or otherwise pointed to by the current resource
        /// </summary>
        /// <value>The URIs of the referenced resources.</value>
        public IList<Uri> References
        {
            get { return this.references; }
        }

        /// <summary>
        /// Gets resources required by the current resource to support its function, delivery or coherence of content
        /// </summary>
        /// <value>The URIs of the required resources</value>
        public IList<Uri> Requires
        {
            get { return this.requires; }
        }


        /// <summary>
        /// Gets or sets the date the web page will be manually reviewed for continued relevance. The date should be in ISO ccyy-mm-dd format.
        /// </summary>
        public string DateReview
        {
            get
            {
                return this.dateReview;
            }
            set
            {
                this.dateReview = value;
            }
        }

        /// <summary>
        /// Gets or sets the date the web page will automatically be removed from publication. The date should be in ISO ccyy-mm-dd format.
        /// </summary>
        /// <remarks>
        /// This could be used for any database-driven time-sensitive content, such as an events list.
        /// </remarks>
        public string DateAutoRemove
        {
            get
            {
                return this.dateAutoRemove;
            }
            set
            {
                this.dateAutoRemove = value;
            }
        }

        /// <summary>
        /// Gets or sets the date the next version of this document is due to be issued in ISO ccyy-mm-dd format
        /// </summary>
        public string DateNextVersionDue
        {
            get
            {
                return this.dateNextVersionDue;
            }
            set
            {
                this.dateNextVersionDue = value;
            }
        }

        /// <summary>
        /// Gets or sets categories of user for whom the resource is intended
        /// </summary>
        /// <value>A semi-colon-separated list of audiences</value>
        /// <remarks>Do not use Audience unless the resource is prepared with a particular group in mind. If it is for general release, leave it blank.</remarks>
        public string Audience
        {
            get { return this.audience; }
            set { this.audience = value; }
        }


        /// <summary>
        /// Gets or sets the entity or entities responsible for making contributions to the content of the resource.
        /// </summary>
        /// <value>Semi-colon separated list of entities</value>
        /// <remarks>A contributor played an important role in creating a resource, but does not have primary or overall responsibility for the content. 
        /// For example, a content editor is a contributor whereas the team that owns the content being edited is the creator.</remarks>
        public string Contributor
        {
            get { return this.contributor; }
            set { this.contributor = value; }
        }

        /// <summary>
        /// Gets or sets the user or role identifier(s) with local management powers over the resource - eg assignment and maintenance of access control
        /// </summary>
        /// <value>Semi-colon-separated list of user or role identifier(s)</value>
        public string Custodian
        {
            get { return this.custodian; }
            set { this.custodian = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the site's copyright information page
        /// </summary>
        public string CopyrightUrl
        {
            get
            {
                return this.copyrightUrl;
            }
            set
            {
                this.copyrightUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets whether there are exemptions to access to the resource in accordance with the Freedom of Information Act
        /// </summary>
        public bool? FoiExempt
        {
            get { return this.foiExempt; }
            set { this.foiExempt = value; }
        }

        #endregion // Optional e-GMS element properties

        #region Non-e-GMS element properties

        /// <summary>
        /// Gets or sets the boolean flag indicating that the document is perticularly relevant for contact centre use
        /// </summary>
        public bool IsContactDoc
        {
            get { return this.isContactDoc; }
            set { this.isContactDoc = value; }
        }

        /// <summary>
        /// Gets or sets the geographic Easting assoxiated with the posting.
        /// </summary>
        public Int32 Easting
        {
            get { return this.easting; }
            set { this.easting = value; }
        }

        /// <summary>
        /// Gets or sets the geographic Northing assoxiated with the posting.
        /// </summary>
        public Int32 Northing
        {
            get { return this.northing; }
            set { this.northing = value; }
        }

        /// <summary>
        /// Gets the RSS feed URLs.
        /// </summary>
        /// <value>The feed URLs.</value>
        public IList<FeedUrl> RssFeeds
        {
            get { return this.feeds; }
        }

        /// <summary>
        /// Gets the <a href="http://www.opensearch.org/">OpenSearch</a> plugin URLs.
        /// </summary>
        /// <value>The OpenSearch plugins' URLs.</value>
        public IList<OpenSearchUrl> OpenSearchUrls
        {
            get { return this.openSearchUrls; }
        }

        /// <summary>
        /// Gets or sets the URL of the first <a href="http://www.opensearch.org/">OpenSearch</a> plugin.
        /// </summary>
        /// <value>The URL.</value>
        public string OpenSearchUrl
        {
            get
            {
                if (this.openSearchUrls.Count > 0) return this.openSearchUrls[0].Url.ToString();
                else return String.Empty;
            }
            set
            {
                if (this.openSearchUrls.Count == 0) this.openSearchUrls.Add(new OpenSearchUrl());
                this.openSearchUrls[0].Url = new Uri(value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the first <a href="http://www.opensearch.org/">OpenSearch</a> plugin.
        /// </summary>
        /// <value>The title.</value>
        public string OpenSearchTitle
        {
            get
            {
                if (this.openSearchUrls.Count > 0) return this.openSearchUrls[0].Title;
                else return String.Empty;
            }
            set
            {
                if (this.openSearchUrls.Count == 0) this.openSearchUrls.Add(new OpenSearchUrl());
                this.openSearchUrls[0].Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the first RSS feed.
        /// </summary>
        /// <value>The RSS feed URL.</value>
        public string RssFeedUrl
        {
            get
            {
                if (this.feeds.Count > 0) return this.feeds[0].Url.ToString();
                else return String.Empty;
            }
            set
            {
                if (this.feeds.Count == 0) this.feeds.Add(new FeedUrl());
                this.feeds[0].Url = new Uri(value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the first RSS feed.
        /// </summary>
        /// <value>The RSS feed title.</value>
        public string RssFeedTitle
        {
            get
            {
                if (this.feeds.Count > 0) return this.feeds[0].Title;
                else return String.Empty;
            }
            set
            {
                if (this.feeds.Count == 0) this.feeds.Add(new FeedUrl());
                this.feeds[0].Title = value;

            }
        }

        /// <summary>
        /// Gets or sets the relationship of the first RSS feed to the current page.
        /// </summary>
        /// <value>The RSS feed relationship.</value>
        public string RssFeedRelationship
        {
            get
            {
                if (this.feeds.Count > 0) return this.feeds[0].Relationship;
                else return String.Empty;
            }
            set
            {
                if (this.feeds.Count == 0) this.feeds.Add(new FeedUrl());
                this.feeds[0].Relationship = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the site's icon
        /// </summary>
        /// <remarks>
        /// <para>This should be left at the normal location of "/favicon.ico" unless there is a good reason not to, because some
        /// browsers ignore the meta element set by this property and always look for "favicon.ico" in the site root.</para>
        /// <para>Icons must be 16 x 16 pixels.</para>
        /// </remarks>
        public string IconUrl
        {
            get
            {
                return this.iconUrl;
            }
            set
            {
                this.iconUrl = value;
            }
        }


        /// <summary>
        /// Gets or sets a URL which Mozilla-based browsers will pre-fetch, appearing to perform faster
        /// </summary>
        public string PreFetchUrl
        {
            get
            {
                return this.preFetchUrl;
            }
            set
            {
                this.preFetchUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's help page
        /// </summary>
        public string HelpToolTip
        {
            get
            {
                return this.helpToolTip;
            }
            set
            {
                this.helpToolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's search page
        /// </summary>
        public string SearchToolTip
        {
            get
            {
                return this.searchToolTip;
            }
            set
            {
                this.searchToolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's home page
        /// </summary>
        public string HomeToolTip
        {
            get
            {
                return this.homeToolTip;
            }
            set
            {
                this.homeToolTip = value;
            }
        }


        /// <summary>
        /// Gets or sets the URL of the site's help page
        /// </summary>
        public string HelpUrl
        {
            get
            {
                return this.helpUrl;
            }
            set
            {
                this.helpUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the site's search page
        /// </summary>
        public string SearchUrl
        {
            get
            {
                return this.searchUrl;
            }
            set
            {
                this.searchUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the site's home page
        /// </summary>
        public string HomeUrl
        {
            get
            {
                return this.homeUrl;
            }
            set
            {
                this.homeUrl = value;
            }
        }


        /// <summary>
        /// Gets or sets whether this page should show up in search results
        /// </summary>
        public bool IsInSearch
        {
            get
            {
                return this.isInSearch;
            }
            set
            {
                this.isInSearch = value;
            }
        }


        /// <summary>
        /// Gets or sets the Facebook app id. This is required for any Open Graph properties to be rendered.
        /// </summary>
        /// <value>The facebook app id.</value>
        public string FacebookAppId { get; set; }

        /// <summary>
        /// Gets or sets the type for the Open Graph Protocol. See <a href="https://developers.facebook.com/docs/opengraph/">https://developers.facebook.com/docs/opengraph/</a>
        /// </summary>
        /// <value>The type.</value>
        public string OpenGraphType { get; set; }

        /// <summary>
        /// Gets or sets the canonical page URL. The URL of the current request is used if this is not specified.
        /// </summary>
        /// <value>The page URL.</value>
        public Uri PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of an image representing the page.
        /// </summary>
        /// <remarks>Used by the <a href="https://developers.facebook.com/docs/opengraph/">Open Graph Protocol</a></remarks>
        /// <value>The image URL.</value>
        public string PageImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the display name of the site.
        /// </summary>
        /// <value>The name of the site.</value>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets Twitter card type. Defaults to <c>summary</c> if not set.
        /// </summary>
        /// <value>The Twitter card type.</value>
        /// <remarks>See the <a href="https://dev.twitter.com/docs/cards">Twitter Cards documentation</a> for details.</remarks>
        public string TwitterCardType { get; set; }

        /// <summary>
        /// The main Twitter account for the site.
        /// </summary>
        public string TwitterAccount { get; set; }

        /// <summary>
        /// Gets or sets the BS7666 address of the place the page is about.
        /// </summary>
        /// <value>The BS7666 address.</value>
        public BS7666Address BS7666Address { get; set; }

        #endregion // Non-e-GMS element properties


        #region Constructor

        /// <summary>
        /// Control to add e-GMS and other web metadata into the &lt;head&gt;&lt;/head&gt; of a web page
        /// </summary>		
        public MetadataControl()
        {
            object obj = ConfigurationManager.GetSection("EsccWebTeam.Egms/EgmsWebMetadata");
            this.config = (obj != null) ? (EgmsWebMetadataConfig)obj : new EgmsWebMetadataConfig();
            this.TitleLanguage = "en-GB";
            this.TwitterCardType = "summary";
        }


        #endregion // Constructor


        #region HTML/XHTML/Error mode support


        /// <summary>
        /// Gets or sets whether to use XHTML or HTML tag formatting
        /// </summary>
        public bool Xhtml
        {
            get
            {
                return this.tagEnd == " />";
            }
            set
            {
                this.tagEnd = (value) ? " />" : ">";
            }
        }

        /// <summary>
        /// Get the HTML or XHTML tag ending
        /// </summary>
        protected string TagEnd
        {
            get { return this.tagEnd; }
        }

        /// <summary>
        /// Takes a URI and, if it's a virtual URI, makes it an absolute URI
        /// </summary>
        /// <param name="uri">The URI to alter, if appropriate</param>
        /// <returns>The updated URI</returns>
        private string MakeUriAbsolute(string uri)
        {
            if (uri.StartsWith("/"))
            {
                uri = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + uri;
            }

            return uri;
        }

        /// <summary>
        /// Gets or sets whether e-GMS metadata is required for this page. Should be set to <code>false</code> only for test pages.
        /// </summary>
        /// <remarks>
        /// You can also set this property in the query string of a single GET request, eg http://localhost/myapp/mypage.aspx?metadatarequired=false
        /// </remarks>
        public bool MetadataRequired
        {
            get
            {
                if (this.Context.Request.QueryString["MetadataRequired"] != null)
                {
                    try
                    {
                        return Boolean.Parse(this.Context.Request.QueryString["MetadataRequired"]);
                    }
                    catch (FormatException)
                    {
                        return this.metadataRequired;
                    }
                }
                return this.metadataRequired;
            }
            set
            {
                this.metadataRequired = value;
            }
        }

        /// <summary>
        /// Gets the metadata from controls in a template
        /// </summary>
        /// <param name="template">The template</param>
        /// <returns>The controls in the template</returns>
        /// <exception cref="ArgumentNullException" />
        private static Control GetMetadataFromTemplate(ITemplate template)
        {
            if (template == null) throw new ArgumentNullException("template");

            MetadataContainer container = new MetadataContainer();
            template.InstantiateIn(container);
            return container;
        }

        /// <summary>
        /// Handles the PreRenderComplete event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// <para>When Page.Title is not set, if you add an HtmlTitle control to the Controls collection of Page.Header, it will 
        /// render that instead of a blank <c>&lt;title&gt;</c> element. But it must be added after PreRender because you 
        /// can't add controls to the Controls collection of any controls outside this one at earlier stages in the life-cycle.
        /// You get an error message: "The control collection cannot be modified during DataBind, Init, Load, PreRender or Unload phases."</para>
        ///<para>Solution is to stash the HtmlTitle in a field, and hook up this event handler to add it later on in the life-cycle.</para>
        /// </remarks>
        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            // first, remove the existing title control if there is one
            foreach (Control control in this.Page.Header.Controls)
            {
                if (control is HtmlTitle)
                {
                    this.Page.Header.Controls.Remove(control);
                    break;
                }
            }

            // now add the new one
            HtmlTitleWithAttributes pageTitle = new HtmlTitleWithAttributes();
            pageTitle.Attributes["lang"] = this.TitleLanguage;
            pageTitle.Attributes["xml:lang"] = this.TitleLanguage;
            pageTitle.Controls.Add(this.titleControl);
            this.Page.Header.Controls.Add(pageTitle);
        }

        /// <summary>
        /// Applies the title pattern to title control.
        /// </summary>
        /// <param name="titleControl">The title control.</param>
        /// <returns></returns>
        private Control ApplyTitlePatternToTitleControl(Control titleControl)
        {
            // If there's no title pattern, don't change anything
            if (String.IsNullOrEmpty(this.titlePattern)) this.TitlePattern = this.config.TitlePattern;
            if (String.IsNullOrEmpty(this.titlePattern))
            {
                return titleControl;
            }

            // If the title pattern doesn't actually include the title, return the pattern as a control
            int titlePos = this.titlePattern.IndexOf("{0}");
            if (titlePos < 0) return new LiteralControl(this.titlePattern);

            // If the pattern does include the title, add literalcontrols before and after and wrap the whole lot up in a placeholder
            string beforeTitle = this.titlePattern.Substring(0, titlePos);
            string afterTitle = this.titlePattern.Substring(titlePos + 3);
            PlaceHolder ph = new PlaceHolder();
            if (beforeTitle.Length > 0) ph.Controls.Add(new LiteralControl(beforeTitle));
            ph.Controls.Add(titleControl);
            if (afterTitle.Length > 0) ph.Controls.Add(new LiteralControl(afterTitle));
            return ph;
        }

        #endregion // HTML/XHTML/Template/Error mode support

        /// <summary>
        /// Builds the metadata from the assigned properties
        /// </summary>
        /// <exception cref="EsccWebTeam.Egms.EgmsMissingMetadataException">
        /// <para>This is thrown when no value is provided for metadata which is mandatory for websites in e-GMS v3.0, and a default value is not available.</para>
        /// <para>You can control when exceptions are thrown by setting the <b>errorMode</b> attribute in web.config, 
        /// which expects a value of <see cref="EsccWebTeam.Egms.MetadataErrorMode" />. 
        /// See <see cref="EsccWebTeam.Egms.EgmsWebMetadataConfig"/> for details of config settings.</para>
        /// </exception>
        protected override void CreateChildControls()
        {
            StringBuilder sb = new StringBuilder();
            HttpContext con = HttpContext.Current;

            // Don't set the encoding, because it needs to be as early as possible whereas this control may not be so early
            // sb.Append("<meta charset=\"UTF-8\"").Append(this.tagEnd).Append(Environment.NewLine);

            // Say we're using Dublin Core metadata
            sb.Append("<link rel=\"schema.DC\" href=\"http://purl.org/dc/elements/1.1/\"").Append(this.tagEnd).Append(Environment.NewLine);
            sb.Append("<link rel=\"schema.DCTERMS\" href=\"http://purl.org/dc/terms/\"").Append(this.tagEnd).Append(Environment.NewLine);
            this.Controls.Add(new LiteralControl(sb.ToString()));
            sb.Length = 0;

            #region Mandatory e-GMS elements

            #region Title
            //
            // This can come from several places, so look for it in order of priority:
            //
            // 1. Page.Title property
            // 2. Title property of this control (only applicable if Page.Title unavailable, eg if the page does not have <head runat="server"> or if this.Title is set at Init before Page.Title exists)
            // 3. TitleTemplate property
            //
            if (!String.IsNullOrEmpty(this.Title))
            {
                // Sanitise title
                this.Title = HttpUtility.HtmlEncode(Html.StripTags(this.Title));

                // Apply the title pattern
                if (String.IsNullOrEmpty(this.titlePattern)) this.TitlePattern = this.config.TitlePattern;
                if (!String.IsNullOrEmpty(this.titlePattern))
                {
                    this.Title = String.Format(this.titlePattern, this.Title);
                }

                // Let .NET emit the <title> element using the Page.Title property, and we'll just deal with the Dublin Core version
                this.Controls.Add(new LiteralControl("<meta name=\"DC.title\" content=\"" + this.Title + "\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));

                // And the Open Graph Protocol version
                if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
                {
                    this.Controls.Add(new LiteralControl("<meta property=\"og:title\" content=\"" + this.Title + "\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));
                }

                // And the Twitter version
                this.Controls.Add(new LiteralControl("<meta name=\"twitter:title\" content=\"" + this.Title + "\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));
            }
            else
            {
                Control titleControl = null;
                Control titleControl2 = null;
                Control titleControl3 = null;
                Control titleControl4 = null;

                if (this.titleTemplate != null)
                {
                    titleControl = MetadataControl.GetMetadataFromTemplate(this.titleTemplate);

                    // If there was nothing in the template, set the control to null.
                    // Otherwise get a second copy of the content in the template.
                    if (titleControl.Controls.Count == 0)
                    {
                        titleControl = null;
                    }
                    else
                    {
                        titleControl2 = MetadataControl.GetMetadataFromTemplate(this.titleTemplate);
                        titleControl3 = MetadataControl.GetMetadataFromTemplate(this.titleTemplate);
                        titleControl4 = MetadataControl.GetMetadataFromTemplate(this.titleTemplate);
                    }
                }

                // If there was nothing in the template, but the title property of this
                // control was set, use that as if it had come from the template
                if (titleControl == null && this.title.Length > 0)
                {
                    var encodedTitle = HttpUtility.HtmlEncode(this.title);
                    titleControl = new LiteralControl(encodedTitle);
                    titleControl2 = new LiteralControl(encodedTitle);
                    titleControl3 = new LiteralControl(encodedTitle);
                    titleControl4 = new LiteralControl(encodedTitle);
                }

                // If there's a control from either the template or the title property, use it
                // Otherwise there's no title, so throw an exception if errors are enabled
                if (titleControl != null)
                {
                    if (this.Page.Header != null)
                    {
                        // If the Page.Title property exists because your page has a <head runat="server"> element, but the Page.Title
                        // property is blank and the page title is instead being set based on the content of the template.
                        // Important to set the value of Page.Title because otherwise it will render a blank <title> element.

                        // But how? Calling the control's render method to get an HTML string, which could be asssigned to Page.Title,
                        // caused problems with CAS permissions.

                        // If you add an HtmlTitle control to the Controls collection of Page.Header, it will render that instead of the 
                        // blank <title> element. But you can't add it here because you can't add controls to the Controls collection of 
                        // any controls outside this one at this stage in the life-cycle. You get an error message: "The control collection 
                        // cannot be modified during DataBind, Init, Load, PreRender or Unload phases."

                        // Solution is to stash the control in a field, and hook up an event handler to run the code we want later on in 
                        // the life-cycle, after PreRender.

                        this.titleControl = this.ApplyTitlePatternToTitleControl(titleControl);
                        this.Page.PreRenderComplete += new EventHandler(Page_PreRenderComplete);

                        // When using a ContentPlaceholder on the applciation.master in SharePoint it seems the PreRenderComplete event 
                        // doesn't fire, but calling .ToString() on the control works. Hopefully won't cause problems elsewhere because 
                        // the code in Page_PreRenderComplete should overwrite this value anyway.
                        this.Page.Title = this.titleControl.ToString();
                    }
                    else
                    {
                        // Otherwise generate our own <title> element
                        HtmlGenericControl titleElement = new HtmlGenericControl("title");
                        titleElement.Attributes["lang"] = this.TitleLanguage;
                        titleElement.Attributes["xml:lang"] = this.TitleLanguage;
                        titleElement.Controls.Add(this.ApplyTitlePatternToTitleControl(titleControl));
                        this.Controls.Add(titleElement);
                        this.Controls.Add(new LiteralControl(Environment.NewLine));
                    }

                    // Either way, generate the Dublin Core tag
                    this.Controls.Add(new LiteralControl("<meta name=\"DC.title\" content=\""));
                    this.Controls.Add(titleControl2);
                    this.Controls.Add(new LiteralControl("\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));

                    // And the Open Graph Protocol version
                    if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
                    {
                        this.Controls.Add(new LiteralControl("<meta property=\"og:title\" content=\""));
                        this.Controls.Add(titleControl3);
                        this.Controls.Add(new LiteralControl("\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));
                    }

                    // And the Twitter version
                    this.Controls.Add(new LiteralControl("<meta property=\"twitter:title\" content=\""));
                    this.Controls.Add(titleControl4);
                    this.Controls.Add(new LiteralControl("\" lang=\"" + this.TitleLanguage + "\" xml:lang=\"" + this.TitleLanguage + "\"" + this.tagEnd + Environment.NewLine));
                }
                else if (this.MetadataRequired && this.config.ThrowErrors)
                {
                    EgmsMissingMetadataException ex = new EgmsMissingMetadataException();
                    ex.SetMissingMetadataField("Title");
                    throw ex;
                }
            }
            #endregion // Title

            #region Creator

            //			if (this.creatorTemplate != null) this.Creator = EgmsWebMetadataControl.GetMetadataFromTemplate(this.creatorTemplate);
            if (this.creator.Length == 0) this.creator = this.config.Creator;
            if (this.creator.Length == 0) this.creator = this.DefaultCreator;
            if (this.creator.Length > 0)
            {
                sb.Append("<meta name=\"DC.creator\" content=\"").Append(HttpUtility.HtmlEncode(this.creator)).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }
            else if (this.MetadataRequired && this.config.ThrowErrors)
            {
                EgmsMissingMetadataException ex = new EgmsMissingMetadataException();
                ex.SetMissingMetadataField("Creator");
                throw ex;
            }
            #endregion // Creator

            #region Date

            // ESD suggests date.issued is required - we'll be a little more flexible and ask for one of the first three
            int dateCount = 0;
            if (this.dateCreated.Length == 0) this.dateCreated = this.config.DateCreated;
            if (this.dateCreated.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.created\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateCreated).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                dateCount++;
            }
            if (this.dateIssued.Length == 0) this.dateIssued = this.config.DateIssued;
            if (this.dateIssued.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.issued\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateIssued).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                dateCount++;
            }
            if (this.dateModified.Length == 0) this.dateModified = this.config.DateModified;
            if (this.dateModified.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.modified\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateModified).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                dateCount++;
            }
            if (dateCount == 0 && this.MetadataRequired && this.config.ThrowErrors)
            {
                EgmsMissingMetadataException ex = new EgmsMissingMetadataException();
                ex.SetMissingMetadataField("Date (at least one of date created, date issued and date modified)");
                throw ex;
            }

            #endregion // Date

            #region Subject

            // IPSV preferred terms
            //			if (this.ipsvPreferredTermsTemplate != null) this.IpsvPreferredTerms = EgmsWebMetadataControl.GetMetadataFromTemplate(this.ipsvPreferredTermsTemplate);
            if (this.ipsvPreferredTerms == null || this.ipsvPreferredTerms.Length == 0) this.IpsvPreferredTerms = this.config.IpsvPreferredTerms;
            if (this.ipsvPreferredTerms != null && this.ipsvPreferredTerms.Length > 0)
            {
                foreach (string term in this.ipsvPreferredTerms)
                {
                    if (term.Length > 0) sb.Append("<meta name=\"DC.subject\" class=\"eGMS.IPSV\" content=\"").Append(term.Trim()).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }
            else if (this.MetadataRequired && this.config.ThrowErrors)
            {
                EgmsMissingMetadataException ex = new EgmsMissingMetadataException();
                ex.SetMissingMetadataField("IPSV preferred terms");
                throw ex;
            }

            // Education and Skills Thesaurus terms
            if (this.educationAndSkillsPreferredTerms == null || this.educationAndSkillsPreferredTerms.Length == 0) this.EducationAndSkillsPreferredTerms = this.config.EducationAndSkillsPreferredTerms;
            if (this.educationAndSkillsPreferredTerms != null && this.educationAndSkillsPreferredTerms.Length > 0)
            {
                foreach (string term in this.educationAndSkillsPreferredTerms)
                {
                    if (term.Length > 0) sb.Append("<meta name=\"DC.subject\" class=\"eGMS.EducationAndSkillsThesaurus\" content=\"").Append(term.Trim()).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }


            #endregion // Subject

            #region Accessibility

            if (this.wcagLevel == WcagConformance.Unknown) this.wcagLevel = this.config.WcagLevel;
            if (this.wcagLevel != WcagConformance.Unknown && this.wcagLevel != WcagConformance.None)
            {
                string wcag = null;
                if (this.wcagLevel == WcagConformance.A || this.wcagLevel == WcagConformance.Wcag2A) wcag = "A";
                else if (this.wcagLevel == WcagConformance.AA || this.wcagLevel == WcagConformance.Wcag2AA) wcag = "Double-A";
                else if (this.wcagLevel == WcagConformance.AAA || this.wcagLevel == WcagConformance.Wcag2AAA) wcag = "Triple-A";

                string scheme = null;
                if (this.wcagLevel == WcagConformance.A || this.wcagLevel == WcagConformance.AA || this.wcagLevel == WcagConformance.AAA) scheme = "eGMS.WCAG10";
                else scheme = "eGMS.WCAG20";

                sb.Append("<meta name=\"eGMS.accessibility\" class=\"").Append(scheme).Append("\" content=\"").Append(wcag).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }


            #endregion //Accessibility

            #region Identifier

            bool fallbackToUrl = true;
            if (this.systemId.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.systemID\" content=\"").Append(this.systemId).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                fallbackToUrl = false;
            }
            if (this.filePlanId.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.fileplanID\" content=\"").Append(this.filePlanId).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                fallbackToUrl = false;
            }

            // Use URL if there's no better identifier, but don't use if another is available as URLs are likely to change
            string pageUrl = HttpUtility.HtmlEncode(this.PageUrl != null ? this.PageUrl.ToString() : this.Context.Request.Url.ToString());
            if (fallbackToUrl)
            {
                sb.Append("<meta name=\"DC.identifier\" class=\"DCTERMS.URI\" content=\"").Append(pageUrl).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Try not to have multiple URLs for a page - use 301 redirects to go to the canonical URL. But for short URLs which are 303
            // redirects, having the canonical URL at the destination should stop the short URL getting into Google's index.
            sb.Append("<meta rel=\"canonical\" content=\"").Append(pageUrl).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);

            // Use page URL for Open Graph Protocol.
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                sb.Append("<meta property=\"og:url\" content=\"").Append(pageUrl).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Identifier

            #region Publishers

            //			if (this.publishersTemplate != null) this.Publisher = EgmsWebMetadataControl.GetMetadataFromTemplate(this.publishersTemplate);
            if (this.publishers == null || this.publishers.Length == 0) this.Publisher = this.config.Publisher;
            if (this.publishers == null || this.publishers.Length == 0) this.Publisher = "East Sussex County Council";
            if (this.publishers != null)
            {
                foreach (string publisher in this.publishers)
                {
                    if (publisher.Length > 0)
                    {
                        sb.Append("<meta name=\"DC.publisher\" content=\"")
                            .Append(HttpUtility.HtmlEncode(publisher))
                            .Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                    }
                }
            }

            #endregion // Publishers


            #endregion // Mandatory e-GMS elements

            #region Recommended e-GMS elements

            #region Coverage

            //if (this.spatialCoverageTemplate != null) this.SpatialCoverage = EgmsWebMetadataControl.GetMetadataFromTemplate(this.spatialCoverageTemplate);
            if (this.spatialCoverage.Length == 0) this.SpatialCoverage = this.config.SpatialCoverage;
            if (this.spatialCoverage.Length == 0) this.SpatialCoverage = "East Sussex, UK";
            if (this.spatialCoverage.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.spatial\" content=\"").Append(HttpUtility.HtmlEncode(this.spatialCoverage));
                if (!this.spatialCoverage.EndsWith(" UK")) sb.Append(", UK");
                sb.Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Postcode can be used for spatial coverage, but don't look for a value in config because very unlikley to be more than one page about one postcode.
            if (this.postcodes.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.spatial\" class=\"eGMS.RoyalMailPostcode\" content=\"").Append(HttpUtility.HtmlEncode(this.postcodes)).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // temporal coverage
            if (this.temporalCoverage.Length == 0) this.temporalCoverage = this.config.TemporalCoverage;
            if (this.temporalCoverage.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.temporal\" content=\"").Append(this.temporalCoverage).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Coverage

            #region Language

            if (this.language.Length == 0) this.language = this.config.Language;
            if (this.language.Length == 0) this.language = "eng";
            if (this.language.Length > 0)
            {
                sb.Append("<meta name=\"DC.language\" class=\"DCTERMS.ISO639-2\" content=\"")
                    .Append(HttpUtility.HtmlEncode(this.language))
                    .Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Language

            #endregion // Recommended e-GMS elements

            #region Optional e-GMS elements

            #region Description

            //if (this.descriptionTemplate != null) this.Description = EgmsWebMetadataControl.GetMetadataFromTemplate(this.descriptionTemplate);
            if (this.description.Length > 0 && this.description.GetHashCode() != this.title.GetHashCode())
            {
                string desc = HttpUtility.HtmlEncode(this.description);
                sb.Append("<meta name=\"DC.description\" content=\"").Append(desc).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                sb.Append("<meta name=\"description\" content=\"").Append(desc).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);

                // Add yet another version for Open Graph Protocol
                if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
                {
                    sb.Append("<meta property=\"og:description\" content=\"").Append(desc).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                }

                // And another for Twitter
                sb.Append("<meta name=\"twitter:description\" content=\"").Append(desc).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }
            #endregion // Description

            #region Subject - uncontrolled keywords

            // IPSV non-preferred terms - appended to config rather than replacing config
            StringBuilder ipsvKeys = new StringBuilder(this.config.IpsvNonPreferredTerms);
            //if (this.ipsvNonPreferredTermsTemplate != null) this.IpsvNonPreferredTerms = EgmsWebMetadataControl.GetMetadataFromTemplate(this.ipsvNonPreferredTermsTemplate);
            if (ipsvKeys.Length > 0 && !ipsvKeys.ToString().EndsWith(";") && this.IpsvNonPreferredTerms.Length > 0) ipsvKeys.Append(";");
            ipsvKeys.Append(this.IpsvNonPreferredTerms);

            // Education and Skills Thesaurus non-preferred terms - appended to config rather than replacing config
            StringBuilder eduKeys = new StringBuilder(this.config.EducationAndSkillsNonPreferredTerms);
            if (eduKeys.Length > 0 && !eduKeys.ToString().EndsWith(";") && this.educationAndSkillsNonPreferredTerms.Length > 0) eduKeys.Append(";");
            eduKeys.Append(this.EducationAndSkillsNonPreferredTerms);

            // uncontrolled keywords - appended to config rather than replacing config
            StringBuilder keys = new StringBuilder(this.config.Keywords);
            //if (this.keywordsTemplate != null) this.Keywords = EgmsWebMetadataControl.GetMetadataFromTemplate(this.keywordsTemplate);
            if (keys.Length > 0 && !keys.ToString().EndsWith(";") && this.keywords.Length > 0) keys.Append(";");
            keys.Append(this.keywords);

            // combine all kinds of keywords, since they're all effectively uncontrolled
            if (ipsvKeys.Length > 0 && keys.Length > 0 && !ipsvKeys.ToString().EndsWith(";")) ipsvKeys.Append(";");
            keys.Insert(0, ipsvKeys.ToString());
            if (eduKeys.Length > 0 && keys.Length > 0 && !eduKeys.ToString().EndsWith(";")) eduKeys.Append(";");
            keys.Insert(0, eduKeys.ToString());

            if (keys.Length > 0)
            {
                sb.Append("<meta name=\"DC.subject\" content=\"").Append(HttpUtility.HtmlEncode(keys.ToString())).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // add the controlled keywords to create an old-fashioned keywords tag for internal search engines
            if (this.ipsvPreferredTerms != null && this.ipsvPreferredTerms.Length > 0)
            {
                if (keys.Length > 0) keys.Insert(0, ";");
                keys.Insert(0, this.IpsvPreferredTerms);
            }

            if (this.educationAndSkillsPreferredTerms != null && this.educationAndSkillsPreferredTerms.Length > 0)
            {
                if (keys.Length > 0) keys.Insert(0, ";");
                keys.Insert(0, this.EducationAndSkillsPreferredTerms);
            }

            if (keys.Length > 0)
            {
                // semi-colons replaced with commas to satisfy SiteMorse's whim
                sb.Append("<meta name=\"keywords\" content=\"").Append(HttpUtility.HtmlEncode(keys.Replace(";", ",").ToString())).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Subject - uncontrolled keywords

            #region Subject - Person
            if (this.person.Length == 0) this.person = this.config.Person;
            if (this.person.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.person\" content=\"").Append(this.person).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Add Open Graph Protocol properties for the person's contact details
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                if (this.PersonAbout != null)
                {
                    foreach (var email in this.PersonAbout.EmailAddresses)
                    {
                        sb.Append("<meta property=\"og:email\" content=\"").Append(HttpUtility.HtmlEncode(email.EmailAddress)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    }
                    foreach (var phone in this.PersonAbout.TelephoneNumbers)
                    {
                        sb.Append("<meta property=\"og:phone_number\" content=\"").Append(HttpUtility.HtmlEncode(phone.ToString())).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    }
                    foreach (var fax in this.PersonAbout.FaxNumbers)
                    {
                        sb.Append("<meta property=\"og:fax_number\" content=\"").Append(HttpUtility.HtmlEncode(fax.ToString())).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    }
                }
            }

            #endregion // Subject - Person

            #region Subject - Place
            // Add Open Graph Protocol properties for the place the page is about
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                if (this.BS7666Address != null && this.BS7666Address.HasAddress())
                {
                    // The street address is saon + paon + street + locality
                    // Create a copy with the other fields missing so we can use the standard rules to build it
                    var streetAddress = new BS7666Address(this.BS7666Address.Paon, this.BS7666Address.Saon, this.BS7666Address.StreetName, this.BS7666Address.Locality, String.Empty, String.Empty, String.Empty);

                    sb.Append("<meta property=\"og:street-address\" content=\"").Append(HttpUtility.HtmlEncode(streetAddress.GetSimpleAddress().ToString(","))).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    if (!String.IsNullOrEmpty(this.BS7666Address.Town)) sb.Append("<meta property=\"og:locality\" content=\"").Append(HttpUtility.HtmlEncode(this.BS7666Address.Town)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    if (!String.IsNullOrEmpty(this.BS7666Address.AdministrativeArea)) sb.Append("<meta property=\"og:region\" content=\"").Append(HttpUtility.HtmlEncode(this.BS7666Address.AdministrativeArea)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    if (!String.IsNullOrEmpty(this.BS7666Address.Postcode)) sb.Append("<meta property=\"og:postal-code\" content=\"").Append(HttpUtility.HtmlEncode(this.BS7666Address.Postcode)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                    sb.Append("<meta property=\"og:country-name\" content=\"UK\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }
            #endregion

            #region Subject - LGSL

            //			if (this.lgslNumbersTemplate != null) this.LgslNumbers = EgmsWebMetadataControl.GetMetadataFromTemplate(this.lgslNumbersTemplate);
            if (this.lgslNumbers == null || this.lgslNumbers.Length == 0) this.LgslNumbers = this.config.LgslNumbers;
            if (this.lgslNumbers != null && this.lgslNumbers.Length > 0)
            {
                // As of e-GMS 3.1 LGSL data can optionally include the term names in addition to the numbers, so code needs to support the following formats:
                //
                // 1; 2; 3
                // 1; Service one; 2; Service two; 3; Service 3
                // 1; Service one; 2; 3; Service three

                string currentValue = String.Empty;
                Regex isNumeric = new Regex("^ *[0-9]+ *$");

                foreach (string lgslValue in this.lgslNumbers)
                {
                    if (lgslValue.Length > 0)
                    {
                        // If this is an LGSL service ID...
                        if (isNumeric.IsMatch(lgslValue))
                        {
                            // Since the service name must follow the number this must be a new service, so output any previously saved service
                            if (currentValue.Length > 0)
                            {
                                sb.Append("<meta name=\"DC.subject\" class=\"eGMS.LGSL\" content=\"").Append(currentValue.Trim()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                            }

                            // Save details of this service in case there's a service name to follow
                            currentValue = lgslValue.Trim();
                        }
                        else if (currentValue.Length > 0)
                        {
                            // If it's non-numeric it only makes sense if it follows a number, which would've been saved in currentValue. Output both together.
                            currentValue += "; " + lgslValue.Trim();
                            sb.Append("<meta name=\"DC.subject\" class=\"eGMS.LGSL\" content=\"").Append(currentValue).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);

                            // Reset saved value
                            currentValue = String.Empty;
                        }
                    }
                }

                // If the last service was a number with no service name it will have been saved but not output, so output it
                if (currentValue.Length > 0)
                {
                    sb.Append("<meta name=\"DC.subject\" class=\"eGMS.LGSL\" content=\"").Append(currentValue.Trim()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }

            #endregion // Subject - LGSL

            #region Subject - LGIL

            //			if (this.lgilTypeTemplate != null) this.LgilType = EgmsWebMetadataControl.GetMetadataFromTemplate(this.lgilTypeTemplate);
            if (this.lgilType.Length == 0) this.lgilType = this.config.LgilType;
            if (this.lgilType.Length > 0)
            {
                sb.Append("<meta name=\"DC.subject\" class=\"eGMS.LGIL\" content=\"").Append(this.lgilType).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Subject - LGIL

            #region Audience

            // Can supply generic audience which is not stated to conform to a particular scheme.
            // Using this format for Active Directory data since there's no declared scheme name to use.
            if (String.IsNullOrEmpty(this.audience)) this.audience = this.config.Audience;
            if (!String.IsNullOrEmpty(this.audience))
            {
                sb.Append("<meta name=\"DCTERMS.audience\" content=\"").Append(this.audience).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Audience

            #region Contributors

            if (String.IsNullOrEmpty(this.contributor)) this.contributor = this.config.Contributor;
            if (!String.IsNullOrEmpty(this.contributor))
            {
                string[] contributors = this.contributor.Split(';');
                foreach (string entity in contributors)
                {
                    string trimmedEntity = entity.Trim();
                    if (trimmedEntity.Length > 0) sb.Append("<meta name=\"DC.contributor\" content=\"").Append(trimmedEntity).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }

            #endregion // Contributors

            #region Type

            if (this.lgtlType.Length == 0) this.lgtlType = this.config.LgtlType;
            if (this.lgtlType.Length > 0)
            {
                sb.Append("<meta name=\"DC.type\" class=\"eGMS.LGTL\" content=\"").Append(this.lgtlType).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // And the Open Graph Protocol version
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                if (String.IsNullOrEmpty(this.OpenGraphType)) this.OpenGraphType = this.config.OpenGraphType;
                if (!String.IsNullOrEmpty(this.OpenGraphType))
                {
                    sb.Append("<meta property=\"og:type\" content=\"").Append(this.OpenGraphType).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }

            #endregion

            #region Format

            if (this.format.Length == 0) this.format = con.Response.ContentType; // "text/html";
            if (this.format.Length > 0)
            {
                sb.Append("<meta name=\"DC.format\" class=\"DCTERMS.IMT\" content=\"").Append(this.format).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }
            #endregion // Format

            #region More dates

            if (this.dateNextVersionDue.Length == 0) this.dateNextVersionDue = this.config.DateNextVersionDue;
            if (this.dateNextVersionDue.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.nextVersionDue\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateNextVersionDue).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.dateAutoRemove.Length == 0) this.dateAutoRemove = this.config.DateAutoRemove;
            if (this.dateAutoRemove.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.autoRemoveDate\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateAutoRemove).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.dateReview.Length == 0) this.dateReview = this.config.DateReview;
            if (this.dateReview.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.disposalReview\" class=\"DCTERMS.W3CDTF\" content=\"").Append(this.dateReview).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // More dates

            #region Document versions and references

            if (this.replacesUrl.Length == 0 && this.config.ReplacesUrl != null) this.replacesUrl = this.config.ReplacesUrl.ToString();
            if (this.replacesUrl.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.replaces\" class=\"DCTERMS.URI\" content=\"").Append(this.MakeUriAbsolute(this.replacesUrl)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.isReplacedByUrl.Length == 0 && this.config.IsReplacedByUrl != null) this.isReplacedByUrl = this.config.IsReplacedByUrl.ToString();
            if (this.isReplacedByUrl.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.isReplacedBy\" class=\"DCTERMS.URI\" content=\"").Append(this.MakeUriAbsolute(this.isReplacedByUrl)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.isPartOfUrl.Length == 0 && this.config.IsPartOfUrl != null) this.isPartOfUrl = this.config.IsPartOfUrl.ToString();
            if (this.isPartOfUrl.Length > 0)
            {
                sb.Append("<meta name=\"DCTERMS.isPartOf\" class=\"DCTERMS.URI\" content=\"").Append(this.MakeUriAbsolute(this.isPartOfUrl)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // URIs of documents referenced by or required by this resource.
            // TODO: Currently these can only be set in the object model, not on the page or in web.config
            foreach (Uri resourceUri in this.references)
            {
                sb.Append("<meta name=\"DCTERMS.references\" class=\"DCTERMS.URI\" content=\"").Append(this.MakeUriAbsolute(resourceUri.ToString())).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            foreach (Uri resourceUri in this.requires)
            {
                sb.Append("<meta name=\"DCTERMS.requires\" class=\"DCTERMS.URI\" content=\"").Append(this.MakeUriAbsolute(resourceUri.ToString())).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Document versions and references

            #region Rights

            if (this.copyrightUrl.Length == 0 && this.config.CopyrightUrl != null) this.copyrightUrl = this.config.CopyrightUrl.ToString();
            if (this.copyrightUrl.Length > 0)
            {
                sb.Append("<meta name=\"eGMS.copyright\" class=\"DCTERMS.URI\" content=\"").Append(this.copyrightUrl).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                sb.Append("<link rel=\"copyright\" href=\"").Append(this.copyrightUrl).Append("\" title=\"Copyright information for this page\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (String.IsNullOrEmpty(this.custodian)) this.custodian = this.config.Custodian;
            if (!String.IsNullOrEmpty(this.custodian))
            {
                sb.Append("<meta name=\"eGMS.custodian\" content=\"").Append(this.custodian).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }


            if (this.foiExempt == null) this.foiExempt = this.config.FoiExempt;
            if (this.foiExempt != null)
            {
                sb.Append("<meta name=\"eGMS.FOIAExemption\" content=\"").Append((bool)this.foiExempt ? "Y" : "N").Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Rights

            #endregion // OPTIONAL e-GMS elements

            #region Non-e-GMS elements

            // non-standard info

            // IsContactDoc
            if (this.isContactDoc == true)
            {
                sb.Append("<meta name=\"ESCC.IsContactDoc\" content=\"").Append(this.isContactDoc.ToString()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Geo.Easting
            if (this.easting > 0)
            {
                sb.Append("<meta name=\"Geo.Easting\" content=\"").Append(this.easting.ToString()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Geo.Northing
            if (this.northing > 0)
            {
                sb.Append("<meta name=\"Geo.Northing\" content=\"").Append(this.northing.ToString()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // shortcut icon/favicon
            if (String.IsNullOrEmpty(this.iconUrl) && this.config.IconUrl != null) this.iconUrl = this.config.IconUrl.ToString();
            if (!String.IsNullOrEmpty(this.iconUrl))
            {
                sb.Append("<link rel=\"shortcut icon\" href=\"").Append(this.iconUrl).Append("\" type=\"image/x-icon\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Apple Touch icon
            // iOS will pick this up automatically just by putting the files in the site root with the right filename.
            // Android 2.2 needs this tag to find the file though, and the tag must have a full file path.
            // Android 2.1 needs the apple-touch-icon-precomposed version of the tag and that also gives us control of the appearance on iOS
            // Source: http://www.ravelrumba.com/blog/android-apple-touch-icon/
            if (config.HasTouchIcon)
            {
                sb.Append("<link rel=\"apple-touch-icon-precomposed\" href=\"").Append(Context.Request.Url.Scheme).Append("://").Append(Context.Request.Url.Host).Append("/apple-touch-icon-precomposed.png\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Windows Tile Icon
            // Used when pinning websites with Internet Explorer in Windows 8 and above
            if (config.WindowsTileIconUrl != null)
            {
                sb.Append("<meta name=\"msapplication-TileImage\" content=\"").Append(HttpUtility.HtmlEncode(config.WindowsTileIconUrl.ToString())).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (!String.IsNullOrEmpty(config.WindowsTileColour))
            {
                sb.Append("<meta name=\"msapplication-TileColor\" content=\"").Append(HttpUtility.HtmlEncode(config.WindowsTileColour)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Add an image for the Open Graph Protocol
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                if (String.IsNullOrEmpty(this.PageImageUrl) && this.config.SiteImageUrl != null) this.PageImageUrl = this.config.SiteImageUrl.ToString();
                if (!String.IsNullOrEmpty(this.PageImageUrl))
                {
                    sb.Append("<meta property=\"og:image\" content=\"").Append(HttpUtility.HtmlEncode(this.PageImageUrl)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }


            // Add site name and app id for Open Graph Protocol
            if (!String.IsNullOrEmpty(this.config.FacebookAppId) || !String.IsNullOrEmpty(this.FacebookAppId))
            {
                if (!String.IsNullOrEmpty(this.config.SiteName))
                {
                    sb.Append("<meta property=\"og:site_name\" content=\"").Append(HttpUtility.HtmlEncode(this.config.SiteName)).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.tagEnd).Append(Environment.NewLine);
                }

                if (String.IsNullOrEmpty(this.FacebookAppId)) this.FacebookAppId = this.config.FacebookAppId;
                sb.Append("<meta property=\"fb:app_id\" content=\"").Append(HttpUtility.HtmlEncode(this.FacebookAppId)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Twitter card type and twitter account
            if (!String.IsNullOrEmpty(this.config.TwitterCardType) || !String.IsNullOrEmpty(this.TwitterCardType))
            {
                if (String.IsNullOrEmpty(this.TwitterCardType) && !String.IsNullOrEmpty(this.config.TwitterCardType)) this.TwitterCardType = this.config.TwitterCardType;
                if (!String.IsNullOrEmpty(this.TwitterCardType))
                {
                    sb.Append("<meta property=\"twitter:card\" content=\"").Append(HttpUtility.HtmlEncode(this.TwitterCardType)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }

            if (!String.IsNullOrEmpty(this.config.TwitterAccount) || !String.IsNullOrEmpty(this.TwitterAccount))
            {
                if (String.IsNullOrEmpty(this.TwitterAccount) && !String.IsNullOrEmpty(this.config.TwitterAccount)) this.TwitterAccount = this.config.TwitterAccount;
                if (!String.IsNullOrEmpty(this.TwitterAccount))
                {
                    var twitterHandle = this.TwitterAccount;
                    if (!twitterHandle.StartsWith("@", StringComparison.Ordinal)) twitterHandle = "@" + twitterHandle;
                    sb.Append("<meta property=\"twitter:site\" content=\"").Append(HttpUtility.HtmlEncode(twitterHandle)).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
                }
            }

            // meta <link /> elements
            if (this.homeUrl.Length == 0 && this.config.HomeUrl != null) this.homeUrl = this.config.HomeUrl.ToString();
            if (this.homeToolTip.Length == 0) this.homeToolTip = this.config.HomeToolTip;
            if (this.homeUrl.Length > 0)
            {
                sb.Append("<link rel=\"first\" href=\"").Append(this.homeUrl).Append("\"");
                if (this.homeToolTip.Length > 0) sb.Append(" title=\"").Append(this.homeToolTip).Append("\"");
                sb.Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.helpUrl.Length == 0 && this.config.HelpUrl != null) this.helpUrl = this.config.HelpUrl.ToString();
            if (this.helpToolTip.Length == 0) this.helpToolTip = this.config.HelpToolTip;
            if (this.helpUrl.Length > 0)
            {
                sb.Append("<link rel=\"help\" href=\"").Append(this.helpUrl).Append("\"");
                if (this.helpToolTip.Length > 0) sb.Append(" title=\"").Append(this.helpToolTip).Append("\"");
                sb.Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (this.searchUrl.Length == 0 && this.config.SearchUrl != null) this.searchUrl = this.config.SearchUrl.ToString();
            if (this.searchToolTip.Length == 0) this.searchToolTip = this.config.SearchToolTip;
            if (this.searchUrl.Length > 0)
            {
                sb.Append("<link rel=\"search\" href=\"").Append(this.searchUrl).Append("\"");
                if (this.searchToolTip.Length > 0) sb.Append(" title=\"").Append(this.searchToolTip).Append("\"");
                sb.Append(this.tagEnd).Append(Environment.NewLine);
            }

            if (!this.IsInSearch)
            {
                sb.Append("<meta name=\"robots\" content=\"none\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // go faster in Firefox
            if (this.preFetchUrl.Length > 0)
            {
                sb.Append("<link rel=\"prefetch\" href=\"").Append(this.preFetchUrl).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // RSS feeds
            foreach (FeedUrl feed in this.feeds)
            {
                sb.Append("<link rel=\"").Append(feed.Relationship).Append("\" type=\"application/rss+xml\" href=\"").Append(feed.Url.ToString()).Append("\" title=\"").Append(feed.Title).Append(" - East Sussex County Council").Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            // OpenSearch
            // Use page-specified URLs in addition to config, not instead of
            if (this.config.OpenSearchUrl != null)
            {
                sb.Append("<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\"").Append(this.config.OpenSearchUrl.Title).Append("\" href=\"").Append(this.config.OpenSearchUrl.Url.ToString()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }
            foreach (OpenSearchUrl search in this.openSearchUrls)
            {
                sb.Append("<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\"").Append(search.Title).Append("\" href=\"").Append(search.Url.ToString()).Append("\"").Append(this.tagEnd).Append(Environment.NewLine);
            }

            #endregion // Non-e-GMS elements

            #region Error mode support

            // don't allow warnings to be turned off lightly!
            if (!this.MetadataRequired)
            {
                // Javascript adds a warning to the top of the page. Embedded here so that independent of site control is applied to.
                // Pages where this appears should never be put live anyway.

                // warnPara.style object used rather than warnPara.setAttribute("style", "...") only because lack of IE support

                sb.Append("<script type=\"text/javascript\">")
                    .Append("function EgmsWebMetadataWarn()")
                    .Append("{")
                    .Append("	if (document.getElementsByTagName)")
                    .Append("	{")
                    .Append("		var body = document.getElementsByTagName('body');")
                    .Append("		if (body) ")
                    .Append("		{")
                    .Append("			body = body[0];")
                    .Append("			var warnPara = document.createElement('p');")
                    .Append("			warnPara.style.backgroundColor = '#900';")
                    .Append("			warnPara.style.color = '#fff';")
                    .Append("			warnPara.style.padding = '.2em';")
                    .Append("			warnPara.style.fontSize = 'small';")
                    .Append("			warnPara.style.fontWeight = 'bold';")
                    .Append("			warnPara.style.position = 'absolute';")
                    .Append("			warnPara.style.zIndex = '100000';")
                    .Append("			warnPara.style.width = '100%';")
                    .Append("			warnPara.appendChild(document.createTextNode('e-GMS metadata warnings have been disabled for this page.'));")
                    .Append("			body.insertBefore(warnPara,body.firstChild);")
                    .Append("		}")
                    .Append("	}")
                    .Append("}")

                    .Append("var mmOldHandler = window.onload;")
                    .Append("if (typeof window.onload != 'function') window.onload = EgmsWebMetadataWarn;")
                    .Append("else window.onload = function() { mmOldHandler(); EgmsWebMetadataWarn(); }")
                    .Append("</script>");
            }


            #endregion // Error mode support

            // spit out all the tags
            this.Controls.Add(new LiteralControl(sb.ToString()));
        }




        #region Render method overrides
        /// <summary>
        /// Don't render any container tag
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {

        }

        /// <summary>
        /// Don't render any container tag
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {

        }
        #endregion // Render method overrides



    }
}

#region Deprecated code

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
    /// <summary>
    /// Obsolete control to add e-GMS and other web metadata into the &lt;head&gt;&lt;/head&gt; of a web page
    /// </summary>
    /// <remarks>
    /// <para>This class is retained for backward compatibility. Use <see cref="MetadataControl"/> instead.</para>
    /// </remarks>
    /// <example>
    /// <para>This class adds the following deprecated attributes of the <b>EgmsWebMetadata</b> element which can be used in <b>web.config</b>.</para>
    /// <code>
    ///		&lt;configSections&gt;
    ///         &lt;sectionGroup name=&quot;EsccWebTeam.Egms&quot;&gt;
    ///			    &lt;section name=&quot;EgmsWebMetadata&quot; type=&quot;EsccWebTeam.Egms.EgmsWebMetadataSectionHandler,EsccWebTeam.Egms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f&quot; /&gt;
    ///         &lt;/sectionGroup&gt;
    ///		&lt;/configSections&gt;
    ///		
    /// 	&lt;EsccWebTeam.Egms&gt;
    /// 	    &lt;EgmsWebMetadata
    ///	            ...		    
    /// 	    	lgalTypes=&quot;Parents and guardians;Educational workers&quot;
    /// 	    	picsLabel=&quot;(pics-1.1 &quot;http://www.icra.org/ratingsv02.html&quot; comment &quot;ICRAonline EN v2.0&quot; l gen true for &quot;http://www.eastsussex.gov.uk/&quot; r (nz 1 vz 1 lz 1 oz 1 cz 1) &quot;http://www.rsac.org/ratingsv01.html&quot; l gen true for &quot;http://www.eastsussex.gov.uk/&quot; r (n 0 s 0 v 0 l 0))&quot;
    /// 		    icraUrl=&quot;http://example.org/icralabels.rdf&quot;
    /// 	    	rnibAccessible=&quot;true&quot;
    /// 		/&gt;
    /// 	&lt;/EsccWebTeam.Egms&gt;
    /// </code>
    /// </example>
    [Obsolete("Use EsccWebTeam.Egms.MetadataControl instead")]
    public class EgmsWebMetadataControl : MetadataControl
    {
        private string picsLabel = String.Empty;
        private string icraUrl = String.Empty;
        private string[] lgalTypes;
        private bool rnibAccessible;
        private EgmsWebMetadataConfig config;

        /// <summary>
        /// Gets or sets the URL of an RDF file containing ICRA labels for the site. ICRA labels can be generated free of charge at <a href="http://www.icra.org">www.icra.org</a>.
        /// </summary>
        public string IcraUrl
        {
            get
            {
                return this.icraUrl;
            }
            set
            {
                this.icraUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the W3C Platform for Internet Content Selection (PICS) label
        /// </summary>
        /// <remarks>
        /// <para>The e-GMS regards the PICS label as part of accessibility. Stating the level of accessibility is mandatory in e-GMS v3.0, and including a PICS label is the minimum requirement.</para>
        /// <para>PICS labels can be generated free of charge at <a href="http://www.icra.org">www.icra.org</a>.</para>
        /// </remarks>
        public string PicsLabel
        {
            get
            {
                return this.picsLabel;
            }
            set
            {
                this.picsLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets a semi-colon separated list of audience types or profiles from the Local Government Audience List (LGAL)
        /// </summary>
        /// <remarks>Always set this property when content is aimed at a specific audience. Leave it blank if your content is 
        /// aimed at a general audience, or one not mentioned in the LGAL. The use of audience types is optional in e-GMS v3.0.</remarks>
        public string LgalTypes
        {
            get
            {
                return (this.lgalTypes != null) ? String.Join(";", this.lgalTypes) : String.Empty;
            }
            set
            {
                this.lgalTypes = (value != null && value.Length > 0) ? value.Trim(';').Split(';') : null;
            }
        }


        /// <summary>
        /// Gets or sets whether this site has the RNIB See it Right accreditation
        /// </summary>
        public bool RnibAccessible
        {
            get
            {
                return this.rnibAccessible;
            }
            set
            {
                this.rnibAccessible = value;
            }
        }

        /// <summary>
        /// This property is retained for backwards compatibility. It is not used.
        /// </summary>
        /// <remarks>The use of GCL categories was mandatory in e-GMS 3.0. Replaced in e-GMS 3.1 by the IPSV.</remarks>
        [Obsolete("The Integrated Public Sector Vocabulary (IPSV) has replaced the Government Category List (GCL). Use the IpsvPreferredTerms property instead.")]
        public string GclCategories
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// This property is retained for backwards compatibility. It is not used.
        /// </summary>
        /// <remarks>The use of IPSV preferred terms is mandatory in e-GMS 3.1. LGCL was previously the most appropriate scheme for local government.</remarks>
        [Obsolete("The Integrated Public Sector Vocabulary (IPSV) has replaced the Local Government Category List (LGCL). Use the IpsvPreferredTerms property instead.")]
        public string LgclCategories
        {
            get { return String.Empty; }
            set { }
        }

        /// <summary>
        /// Builds the metadata from the assigned properties
        /// </summary>
        /// <exception cref="EsccWebTeam.Egms.EgmsMissingMetadataException">
        /// 	<para>This is thrown when no value is provided for metadata which is mandatory for websites in e-GMS v3.0, and a default value is not available.</para>
        /// 	<para>You can control when exceptions are thrown by setting the <b>errorMode</b> attribute in web.config,
        /// which expects a value of <see cref="EsccWebTeam.Egms.MetadataErrorMode"/>.
        /// See <see cref="EsccWebTeam.Egms.EgmsWebMetadataConfig"/> for details of config settings.</para>
        /// </exception>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Output deprecated properties as well
            object obj = ConfigurationManager.GetSection("EsccWebTeam.Egms/EgmsWebMetadata");
            this.config = (obj != null) ? (EgmsWebMetadataConfig)obj : new EgmsWebMetadataConfig();

            StringBuilder sb = new StringBuilder();


            // Errors used to be thrown for a missing PICS label, because that's the minimum requirement stated in the e-GMS 3.1.
            // However the PICS standard is now dead so it doesn't make sense to comply with that requirement.
            // WCAG AA accessibility is required of a site, but stating it seems to be optional in the e-GMS.
            if (this.picsLabel.Length == 0) this.picsLabel = this.config.PicsLabel;
            if (this.picsLabel.Length > 0)
            {
                // use single quotes around the attribute because not sure whether &quot; will be interpreted correctly
                sb.Append("<meta http-equiv=\"pics-label\" content='").Append(this.picsLabel).Append("'").Append(this.TagEnd).Append(Environment.NewLine);
            }

            if (this.icraUrl.Length == 0 && this.config.IcraUrl != null) this.icraUrl = this.config.IcraUrl.ToString();
            if (this.icraUrl.Length > 0)
            {
                sb.Append("<link rel=\"meta\" href=\"").Append(this.icraUrl).Append("\" type=\"application/rdf+xml\" title=\"ICRA labels\"").Append(this.TagEnd).Append(Environment.NewLine);
            }

            if (!this.rnibAccessible) this.rnibAccessible = this.config.RnibAccessible;
            if (this.rnibAccessible)
            {
                sb.Append("<meta name=\"eGMS.accessibility\" class=\"RNIB\" content=\"Approved\"").Append(this.TagEnd).Append(Environment.NewLine);
            }

            // LGAL not mentioned as a scheme in e-GMS 3.1, but is still listed and appears to be under development (at the speed of a snail with a limp) on esd.org.uk as at Oct 2007

            //if (this.lgalTypesTemplate != null) this.LgalTypes = EgmsWebMetadataControl.GetMetadataFromTemplate(this.lgalTypesTemplate);
            if (this.lgalTypes == null || this.lgalTypes.Length == 0) this.LgalTypes = this.config.LgalTypes;
            if (this.lgalTypes != null)
            {
                foreach (string profile in this.lgalTypes)
                {
                    if (profile.Length > 0) sb.Append("<meta name=\"DCTERMS.audience\" class=\"eGMS.LGAL\" content=\"").Append(profile.Trim()).Append("\" lang=\"en-GB\" xml:lang=\"en-GB\"").Append(this.TagEnd).Append(Environment.NewLine);
                }
            }

            this.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}
#endregion