using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace EsccWebTeam.Egms
{
    /// <summary>
    /// Configuration settings for working with e-GMS and other web metadata
    /// </summary>
    [Serializable]
    internal class EgmsWebMetadataConfig
    {
        #region Private fields
        private string creator = String.Empty;
        private string ipsvPreferredTerms = String.Empty;
        private string ipsvNonPreferredTerms = String.Empty;
        private string lgalTypes = String.Empty;
        private string audience = String.Empty;
        private string contributor = String.Empty;
        private string custodian = String.Empty;
        private WcagConformance wcagLevel = WcagConformance.Unknown;
        private string picsLabel = String.Empty;
        private string publisher = String.Empty;
        private string spatialCoverage = String.Empty;
        private string language = String.Empty;
        private string keywords = String.Empty;
        private string lgtlType = String.Empty;
        private MetadataErrorMode errorMode = MetadataErrorMode.On;
        private string dateNextVersionDue = String.Empty;
        private string dateCreated = String.Empty;
        private string dateModified = String.Empty;
        private string dateIssued = String.Empty;
        private string dateAutoRemove = String.Empty;
        private string dateReview = String.Empty;
        private Uri isReplacedByUrl;
        private Uri replacesUrl;
        private string person = String.Empty;
        private string lgslNumbers = String.Empty;
        private string lgilType = String.Empty;
        private Uri homeUrl;
        private Uri searchUrl;
        private Uri helpUrl;
        private Uri copyrightUrl;
        private string homeToolTip = String.Empty;
        private string searchToolTip = String.Empty;
        private string helpToolTip = String.Empty;
        private bool rnibAccessible;
        private string temporalCoverage = String.Empty;
        private bool localRequestSet;
        private bool localRequest;
        private Uri icraUrl;
        private OpenSearchUrl openSearch;
        private Uri iconUrl;
        private string educationAndSkillsPreferredTerms = String.Empty;
        private string educationAndSkillsNonPreferredTerms = String.Empty;
        private bool? foiExempt;
        private Uri isPartOf;

        #endregion // Private fields

        #region Properties

        /// <summary>
        /// Gets or sets whether to link to the standard Apple touch icon.
        /// </summary>
        /// <value><c>true</c> to link to touch icon, <c>false</c> otherwise.</value>
        public bool HasTouchIcon { get; set; }

        /// <summary>
        /// Gets or sets the title pattern.
        /// </summary>
        /// <value>The title pattern.</value>
        public string TitlePattern { get; set; }

        /// <summary>
        /// Gets the URL of a plugin for the <a href="http://www.opensearch.org/">OpenSearch</a> standard.
        /// </summary>
        /// <value>The OpenSearch URL.</value>
        public OpenSearchUrl OpenSearchUrl
        {
            get
            {
                return this.openSearch;
            }
            set
            {
                this.openSearch = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of an RDF file containing ICRA labels for the site. ICRA labels can be generated free of charge at <a href="http://www.icra.org">www.icra.org</a>.
        /// </summary>
        [Obsolete("The ICRA standard is obsolete.")]
        public Uri IcraUrl
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
        /// Gets or sets the resource of which the current resource is a part
        /// </summary>
        /// <value>The URL of the referenced resource.</value>
        public Uri IsPartOfUrl
        {
            get { return this.isPartOf; }
            set { this.isPartOf = value; }
        }


        /// <summary>
        /// Gets or sets the URL of a shortcut icon (favicon) for the site.
        /// </summary>
        public Uri IconUrl
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
        /// Gets or sets a semi-colon separated list of audience types or profiles from the Local Government Audience List (LGAL)
        /// </summary>
        public string LgalTypes
        {
            get
            {
                return this.lgalTypes;
            }
            set
            {
                this.lgalTypes = value;
            }
        }

        /// <summary>
        /// Gets or sets the time period the web page is about
        /// </summary>
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
        /// Gets or sets whether there are exemptions to access to the resource in accordance with the Freedom of Information Act
        /// </summary>
        public bool? FoiExempt
        {
            get { return this.foiExempt; }
            set { this.foiExempt = value; }
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
        /// Gets or sets the URL of the site's copyright information page
        /// </summary>
        public Uri CopyrightUrl
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
        /// Gets or sets the URL of the site's help page
        /// </summary>
        public Uri HelpUrl
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
        public Uri SearchUrl
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
        public Uri HomeUrl
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
        /// Gets or sets the Local Government Interaction Type
        /// </summary>
        /// <remarks>The Local Government Interaction Type List is defined at http://www.esd.org.uk/standards/lgil/</remarks>
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
        /// Gets or sets the numeric process identifiers from the Local Government Service List (LGSL)
        /// </summary>
        /// <remarks>The LGSL is known on the ESD toolkit as the PID List</remarks>
        public string LgslNumbers
        {
            get
            {
                return this.lgslNumbers;
            }
            set
            {
                this.lgslNumbers = value;
            }
        }

        /// <summary>
        /// Gets or sets the person who the web page is about
        /// </summary>
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
        /// Gets or sets an identifier of a resource which this web page replaces
        /// </summary>
        public Uri ReplacesUrl
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
        public Uri IsReplacedByUrl
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
        /// Gets or sets the date the web page will be manually reviewed for continued relevance
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
        /// Gets or sets the date the web page will automatically be removed from publication
        /// </summary>
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
        /// Gets or sets the date the document was published in ISO ccyy-mm-dd format
        /// </summary>
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
        /// Gets or sets whether to throw errors if mandatory metadata is missing
        /// </summary>
        public MetadataErrorMode ErrorMode
        {
            get
            {
                return this.errorMode;
            }
            set
            {
                this.errorMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of content - eg maps, minutes, graphs etc
        /// </summary>
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
        /// Gets or sets the Integrated Public Sector Vocabulary (IPSV) preferred terms for the web page
        /// </summary>
        /// <remarks>Required in e-GMS 3.1. Terms should be separated using semi-colons.</remarks>
        public string IpsvPreferredTerms
        {
            get
            {
                return this.ipsvPreferredTerms;
            }
            set
            {
                this.ipsvPreferredTerms = value;
            }
        }

        /// <summary>
        /// Gets or sets the Integrated Public Sector Vocabulary (IPSV) non-preferred terms for the web page
        /// </summary>
        /// <remarks>Optional in e-GMS 3.1. Terms should be separated using semi-colons.</remarks>
        public string IpsvNonPreferredTerms
        {
            get
            {
                return this.ipsvNonPreferredTerms;
            }
            set
            {
                this.ipsvNonPreferredTerms = value;
            }
        }

        /// <summary>
        /// Gets or sets the Education and Skills Thesaurus preferred terms for the web page
        /// </summary>
        /// <remarks>Optional in e-GMS 3.1. Terms should be separated using semi-colons.</remarks>
        public string EducationAndSkillsPreferredTerms
        {
            get
            {
                return this.educationAndSkillsPreferredTerms;
            }
            set
            {
                this.educationAndSkillsPreferredTerms = value;
            }
        }

        /// <summary>
        /// Gets or sets the Education and Skills Thesaurus non-preferred terms for the web page
        /// </summary>
        /// <remarks>Optional in e-GMS 3.1. Terms should be separated using semi-colons.</remarks>
        public string EducationAndSkillsNonPreferredTerms
        {
            get
            {
                return this.educationAndSkillsNonPreferredTerms;
            }
            set
            {
                this.educationAndSkillsNonPreferredTerms = value;
            }
        }


        /// <summary>
        /// Gets or sets the additional keywords - those not taken from a controlled vocabulary
        /// </summary>
        /// <remarks>Keywords should be separated using semi-colons</remarks>
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
        /// Gets or sets the language 
        /// </summary>
        /// <remarks>Recommended in e-GMS 3</remarks>
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
        /// Gets or sets a semi-colon separated list of publishers - the entities who need to give permission should someone want to republish the data
        /// </summary>
        /// <remarks>This property is called "Publisher" rather than "Publishers" for backwards compatibility - it used only to accept one publisher.</remarks>
        public string Publisher
        {
            get
            {
                return this.publisher;
            }
            set
            {
                this.publisher = value;
            }
        }

        /// <summary>
        /// Gets or sets the W3C Platform for Internet Content Selection (PICS) label
        /// </summary>
        [Obsolete("The PICS standard is obsolete")]
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
        /// Gets or sets level of conformance to the Web Content Accessibility Guidelines 1.0
        /// </summary>
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
        /// Gets or sets the entity primarily responsible for the content of the web page
        /// </summary>
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
        /// Gets or sets the URL of an image representing the page.
        /// </summary>
        /// <remarks>Used by the <a href="https://developers.facebook.com/docs/opengraph/">Open Graph Protocol</a></remarks>
        /// <value>The image URL.</value>
        public Uri SiteImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the display name of the site.
        /// </summary>
        /// <value>The name of the site.</value>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets the Windows 8 tile icon URL.
        /// </summary>
        /// <value>The windows tile icon URL.</value>
        public Uri WindowsTileIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the Windows 8 tile colour.
        /// </summary>
        /// <value>The windows tile colour.</value>
        public string WindowsTileColour { get; set; }

        #endregion // Properties


        #region Error mode support
        /// <summary>
        /// Determines whether errors should be thrown
        /// </summary>
        /// <returns></returns>
        internal bool ThrowErrors
        {
            get
            {
                // Don't throw errors if creating new page in CMS
                // Use querystring to test mode as it means references to CMS DLLs not required
                NameValueCollection qs = HttpContext.Current.Request.QueryString;
                if (qs["WBCMODE"] != null && qs["WBCMODE"].ToString() != "Published") return false;
                if (qs["NRMODE"] != null && qs["NRMODE"].ToString() != "Published") return false;

                // determine whether to throw errors based on config setting
                return (
                    (this.ErrorMode == MetadataErrorMode.On) ||
                    (this.ErrorMode == MetadataErrorMode.RemoteOnly && !this.IsLocalMachine()) ||
                    (this.ErrorMode == MetadataErrorMode.LocalOnly && this.IsLocalMachine())
                    );
            }
        }

        /// <summary>
        /// Gets whether the request is known to come from the same machine
        /// </summary>
        /// <returns>true if it does; false otherwise</returns>
        private bool IsLocalMachine()
        {
            // cache result in private field
            if (this.localRequestSet) return this.localRequest;

            string requestIp = HttpContext.Current.Request.UserHostAddress;

            // localhost is matching IP
            if (requestIp == "127.0.0.1")
            {
                this.localRequest = true;
                this.localRequestSet = true;
                return true;
            }

            // using host name, get IP address list
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());

            for (int i = 0; i < ipEntry.AddressList.Length; i++)
            {
                // look for matching IP
                if (requestIp == ipEntry.AddressList[i].ToString())
                {
                    this.localRequest = true;
                    this.localRequestSet = true;
                    return true;
                }
            }

            this.localRequest = false;
            this.localRequestSet = true;
            return false;
        }
        #endregion // Error mode support

    }
}
