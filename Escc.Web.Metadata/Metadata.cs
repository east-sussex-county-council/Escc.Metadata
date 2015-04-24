using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Escc.AddressAndPersonalDetails;

namespace Escc.Web.Metadata
{
    /// <summary>
    /// E-GMS and other web-related metadata
    /// </summary>
    class Metadata : IMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata"/> class.
        /// </summary>
        public Metadata()
        {
            Audience = String.Empty;
            Contributor = String.Empty;
            CopyrightUrl = String.Empty;
            Creator = String.Empty;
            Custodian = String.Empty;
            DateAutoRemove = String.Empty;
            DateCreated = String.Empty;
            DateIssued = String.Empty;
            DateModified = String.Empty;
            DateNextVersionDue = String.Empty;
            DateReview = String.Empty;
            Description = String.Empty;
            Easting = default(Int32);
            Northing = default(Int32);
            EducationAndSkillsPreferredTerms = String.Empty;
            EducationAndSkillsNonPreferredTerms = String.Empty;
            FilePlanId = String.Empty;
            Format = String.Empty;
            HelpToolTip = String.Empty;
            HelpUrl = String.Empty;
            HomeToolTip = String.Empty;
            HomeUrl = String.Empty;
            IconUrl = String.Empty;
            IpsvNonPreferredTerms = String.Empty;
            IpsvPreferredTerms = String.Empty;
            IsContactDoc = default(bool);
            IsInSearch = true;
            IsPartOfUrl = String.Empty;
            IsReplacedByUrl = String.Empty;
            Keywords = String.Empty;
            Language = String.Empty;
            LgilType = String.Empty;
            LgslNumbers = String.Empty;
            LgtlType = String.Empty;
            OpenSearchUrls = new List<OpenSearchUrl>();
            Person = String.Empty;
            Postcodes = String.Empty;
            PreFetchUrl = String.Empty;
            Publisher = String.Empty;
            References = new List<Uri>();
            Requires = new List<Uri>();
            ReplacesUrl = String.Empty;
            RssFeeds = new List<FeedUrl>();
            SearchToolTip = String.Empty;
            SearchUrl = String.Empty;
            SpatialCoverage = String.Empty;
            SystemId = String.Empty;
            TemporalCoverage = String.Empty;
            Title = String.Empty;
            TitleLanguage = "en-GB";
            TitlePattern = String.Empty;
            FacebookAppId = String.Empty;
            OpenGraphType = String.Empty;
            PageImageUrl = String.Empty;
            SiteName = String.Empty;
            TwitterAccount = String.Empty;
            TwitterCardType = "summary";
        }

        /// <summary>
        /// Gets or sets categories of user for whom the resource is intended
        /// </summary>
        /// <value>A semi-colon-separated list of audiences</value>
        /// <remarks>Do not use Audience unless the resource is prepared with a particular group in mind. If it is for general release, leave it blank.</remarks>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the entity or entities responsible for making contributions to the content of the resource.
        /// </summary>
        /// <value>Semi-colon separated list of entities</value>
        /// <remarks>A contributor played an important role in creating a resource, but does not have primary or overall responsibility for the content. 
        /// For example, a content editor is a contributor whereas the team that owns the content being edited is the creator.</remarks>
        public string Contributor { get; set; }

        /// <summary>
        /// Gets or sets the URL of the site's copyright information page
        /// </summary>
        public string CopyrightUrl { get; set; }

        /// <summary>
        /// Gets or sets the entity primarily responsible for the content of the web page
        /// </summary>
        /// <remarks>
        /// <para>Mandatory in e-GMS v3.0.</para>
        /// <para>Job titles should be used rather than names because they're less likely to change. 
        /// If names are used, Dublin Core recommends the "Lastname, firstname" format.</para>
        /// <para>In addition, give a telephone number or a generic email address. Do not use personal email addresses.</para>
        /// </remarks>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the user or role identifier(s) with local management powers over the resource - eg assignment and maintenance of access control
        /// </summary>
        /// <value>Semi-colon-separated list of user or role identifier(s)</value>
        public string Custodian { get; set; }

        /// <summary>
        /// Gets or sets the date the web page will automatically be removed from publication. The date should be in ISO ccyy-mm-dd format.
        /// </summary>
        /// <remarks>
        /// This could be used for any database-driven time-sensitive content, such as an events list.
        /// </remarks>
        public string DateAutoRemove { get; set; }

        /// <summary>
        /// Gets or sets the date the document was created in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date the document was published in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued (this property) is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the date the document was modified in ISO ccyy-mm-dd format
        /// </summary>
        /// <remarks>
        /// <para>Setting a date is mandatory in e-GMS v3.0.</para>
        /// <para>Unoffical advice on e-GMS implementation for the web suggests that DC.date.issued is the minimum requirement.
        /// <a href="http://www.esd.org.uk/standards/egms/">http://www.esd.org.uk/standards/egms/</a>.</para>
        /// </remarks>
        public string DateModified { get; set; }

        /// <summary>
        /// Gets or sets the date the next version of this document is due to be issued in ISO ccyy-mm-dd format
        /// </summary>
        public string DateNextVersionDue { get; set; }

        /// <summary>
        /// Gets or sets the date the web page will be manually reviewed for continued relevance. The date should be in ISO ccyy-mm-dd format.
        /// </summary>
        public string DateReview { get; set; }

        /// <summary>
        /// Gets or sets the description of the web page
        /// </summary>
        /// <remarks>
        /// <para>Optional in e-GMS v3.0, but it should be set since this is the description which is displayed in search result listings.</para>
        /// <para>Try not to repeat information that could be held in another element, eg title, coverage or subject. The description should 
        /// be short but meaningful (under 30 words). It could include the approach to the subject, the reason it’s produced, groups and 
        /// organisations referred to, events covered, key outcomes, broad policy areas etc.</para>
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the geographic Easting associated with the page.
        /// </summary>
        public int Easting { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Education and Skills Thesaurus non-preferred terms
        /// </summary>
        /// <remarks>The use of the Education and Skills Thesaurus is optional in e-GMS 3.1.</remarks>
        public string EducationAndSkillsNonPreferredTerms { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Education and Skills Thesaurus preferred terms
        /// </summary>
        /// <remarks>
        /// <para>The use of the Education and Skills Thesaurus is optional in e-GMS 3.1.</para>
        /// </remarks>
        public string EducationAndSkillsPreferredTerms { get; set; }

        /// <summary>
        /// Gets or sets the reference derived from the fileplan. This is a culmination of information inherited from higher levels of aggregation in the fileplan.
        /// </summary>
        public string FilePlanId { get; set; }

        /// <summary>
        /// Gets or sets whether there are exemptions to access to the resource in accordance with the Freedom of Information Act
        /// </summary>
        public bool? FoiExempt { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the web page
        /// </summary>
        /// <remarks><para>Optional in e-GMS 3. If no value is specified, "text/html" will be used since it will always be correct for a web page 
        /// as long as Internet Explorer 6 or below is in common use.</para>
        /// <para>XHTML 1.0 web pages can use (and XHTML 1.1 pages <b>must</b> use) "application/xhtml+xml", but Internet Explorer 6 doesn't recognise it.</para>
        /// </remarks>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's help page
        /// </summary>
        public string HelpToolTip { get; set; }

        /// <summary>
        /// Gets or sets the URL of the site's help page
        /// </summary>
        public string HelpUrl { get; set; }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's home page
        /// </summary>
        public string HomeToolTip { get; set; }

        /// <summary>
        /// Gets or sets the URL of the site's home page
        /// </summary>
        public string HomeUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the site's icon
        /// </summary>
        /// <remarks>
        /// <para>This should be left at the normal location of "/favicon.ico" unless there is a good reason not to, because some
        /// browsers ignore the meta element set by this property and always look for "favicon.ico" in the site root.</para>
        /// <para>Icons must be 16 x 16 pixels.</para>
        /// </remarks>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Integrated Public Sector Vocabulary (IPSV) non-preferred terms
        /// </summary>
        /// <remarks>The use of IPSV preferred terms is mandatory in e-GMS 3.1. However, it is not essential to include the non-preferred terms.</remarks>
        public string IpsvNonPreferredTerms { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Integrated Public Sector Vocabulary (IPSV) preferred terms
        /// </summary>
        /// <remarks>
        /// <para>The use of IPSV preferred terms is mandatory in e-GMS 3.1.</para>
        /// <para>Always use the full version of the IPSV to find the relevant term(s). (There is also an abridged version, which is not suitable for local government.)</para>
        /// <para>Choose the term that is most relevant to the content from the complete hierarchy list. You can include more than one term if relevant. 
        /// You don’t need to include all categories as you drill down – just the one that best describes the content on the page.</para>
        /// </remarks>
        public string IpsvPreferredTerms { get; set; }

        /// <summary>
        /// Gets or sets the boolean flag indicating that the document is perticularly relevant for contact centre use
        /// </summary>
        public bool IsContactDoc { get; set; }

        /// <summary>
        /// Gets or sets whether this page should show up in search results
        /// </summary>
        public bool IsInSearch { get; set; }

        /// <summary>
        /// Gets or sets the resource of which the current resource is a part
        /// </summary>
        /// <value>The URL of the referenced resource.</value>
        public string IsPartOfUrl { get; set; }

        /// <summary>
        /// Gets or sets an identifier of a resource which replaced this web page 
        /// </summary>
        /// <remarks>This might be used where an archive of older versions of a document is available, perhaps for a document which is updated annually. 
        /// The property should be set on the archived document, pointing to the URL of the newer document.</remarks>
        public string IsReplacedByUrl { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of additional keywords for the web page - those not taken from a controlled vocabulary
        /// </summary>
        /// <remarks>Optional in e-GMS v3.0.</remarks>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the language of the document using an ISO 639 language code
        /// </summary>
        /// <remarks>Recommended in e-GMS v3.0. If no language is specified, "eng" is used.</remarks>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets an interaction type from the Local Government Interaction Type List (LGIL)
        /// </summary>
        /// <remarks>Most ordinary web pages should use the "Providing information" type. Some web applications will use "Applications for service".</remarks>
        public string LgilType { get; set; }

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
        public string LgslNumbers { get; set; }

        /// <summary>
        /// Gets or sets the type of content on the page - eg maps, minutes, graphs etc - as a value from the Local Government Type List (LGTL)
        /// </summary>
        /// <remarks>
        /// <para>Optional in e-GMS v3.0.</para>
        /// <para>The LGTL is not a mature standard, so you will often find there is no appropriate type. In this case, simply leave the property blank.</para>
        /// </remarks>
        public string LgtlType { get; set; }

        /// <summary>
        /// Gets or sets the geographic Northing associated with the page.
        /// </summary>
        public int Northing { get; set; }

        /// <summary>
        /// Gets or sets the URL of the first <a href="http://www.opensearch.org/">OpenSearch</a> plugin.
        /// </summary>
        /// <value>The URL.</value>
        public string OpenSearchUrl
        {
            get
            {
                if (this.OpenSearchUrls.Count > 0) return this.OpenSearchUrls[0].Url.ToString();
                else return String.Empty;
            }
            set
            {
                if (this.OpenSearchUrls.Count == 0) this.OpenSearchUrls.Add(new OpenSearchUrl());
                this.OpenSearchUrls[0].Url = new Uri(value);
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
                if (this.OpenSearchUrls.Count > 0) return this.OpenSearchUrls[0].Title;
                else return String.Empty;
            }
            set
            {
                if (this.OpenSearchUrls.Count == 0) this.OpenSearchUrls.Add(new OpenSearchUrl());
                this.OpenSearchUrls[0].Title = value;
            }
        }

        /// <summary>
        /// Gets the <a href="http://www.opensearch.org/">OpenSearch</a> plugin URLs.
        /// </summary>
        /// <value>The OpenSearch plugins' URLs.</value>
        public IList<OpenSearchUrl> OpenSearchUrls { get; private set; }

        /// <summary>
        /// Gets or sets the person who the web page is about
        /// </summary>
        /// <example>Councillor John Smith</example>
        public string Person { get; set; }

        /// <summary>
        /// Gets or sets the person the page is about.
        /// </summary>
        /// <value>The person the page is about.</value>
        public Person PersonAbout { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of Royal Mail postcodes which identify the geographical area(s) covered by the web page
        /// </summary>
        /// <remarks>Used as a value for spatial coverge, which is recommended in e-GMS 3.1.</remarks>
        /// <example>BN7 1UE; BN9 9TT</example>
        public string Postcodes { get; set; }

        /// <summary>
        /// Gets or sets a URL which Mozilla-based browsers will pre-fetch, appearing to perform faster
        /// </summary>
        public string PreFetchUrl { get; set; }

        /// <summary>
        /// Gets or sets a semi-colon separated list of publishers - the entities who need to give permission should someone want to republish the data
        /// </summary>
        /// <remarks>
        /// <para>Mandatory for web pages in e-GMS v3.0. Can usually be set to "East Sussex County Council".</para>
        /// <para>This property is called "Publisher" rather than "Publishers" for backwards compatibility - it used only to accept one publisher.</para>
        /// </remarks>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets resources referenced, cited or otherwise pointed to by the current resource
        /// </summary>
        /// <value>The URIs of the referenced resources.</value>
        public IList<Uri> References { get; private set; }

        /// <summary>
        /// Gets resources required by the current resource to support its function, delivery or coherence of content
        /// </summary>
        /// <value>The URIs of the required resources</value>
        public IList<Uri> Requires { get; private set; }

        /// <summary>
        /// Gets or sets an identifier of a resource which this web page replaces
        /// </summary>
        /// <remarks>This might be used where an archive of older versions of a document is available, perhaps for a document which is updated annually. 
        /// The property should be set to the URL of the older document.</remarks>
        public string ReplacesUrl { get; set; }


        /// <summary>
        /// Gets or sets the relationship of the first RSS feed to the current page.
        /// </summary>
        /// <value>The RSS feed relationship.</value>
        public string RssFeedRelationship
        {
            get
            {
                if (RssFeeds.Count > 0) return RssFeeds[0].Relationship;
                else return String.Empty;
            }
            set
            {
                if (RssFeeds.Count == 0) RssFeeds.Add(new FeedUrl());
                RssFeeds[0].Relationship = value;
            }
        }

        /// <summary>
        /// Gets the RSS feed URLs.
        /// </summary>
        /// <value>The feed URLs.</value>
        public IList<FeedUrl> RssFeeds { get; private set; }

        /// <summary>
        /// Gets or sets the URL of the first RSS feed.
        /// </summary>
        /// <value>The RSS feed URL.</value>
        public string RssFeedUrl
        {
            get
            {
                if (RssFeeds.Count > 0) return RssFeeds[0].Url.ToString();
                else return String.Empty;
            }
            set
            {
                if (RssFeeds.Count == 0) RssFeeds.Add(new FeedUrl());
                RssFeeds[0].Url = new Uri(value);
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
                if (RssFeeds.Count > 0) return RssFeeds[0].Title;
                else return String.Empty;
            }
            set
            {
                if (RssFeeds.Count == 0) RssFeeds.Add(new FeedUrl());
                RssFeeds[0].Title = value;

            }
        }

        /// <summary>
        /// Gets or sets the pop-up text for the link to the site's search page
        /// </summary>
        public string SearchToolTip { get; set; }

        /// <summary>
        /// Gets or sets the URL of the site's search page
        /// </summary>
        public string SearchUrl { get; set; }

        /// <summary>
        /// Gets or sets the geographical area covered by the web page
        /// </summary>
        /// <remarks>Recommended in e-GMS v3.0. The coverage should be unique enough to distinguish the 
        /// area from any other similiarly named area in the world. If no area is specified, "East Sussex, UK" will be used.</remarks>
        /// <example>Lewes, East Sussex, UK</example>
        public string SpatialCoverage { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the content in an electronic system - eg a database id.
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// Gets or sets the time period the web page is about
        /// </summary>
        /// <remarks>There is no specific format, so "Tuesdays" is just as valid as "2001-2002".</remarks>
        public string TemporalCoverage { get; set; }

        /// <summary>
        /// Gets or sets the title of the web page
        /// </summary>
        /// <remarks>
        /// <para>Title is mandatory in e-GMS v3.1.</para>
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the language the title is written in.
        /// </summary>
        /// <value>The title language.</value>
        public string TitleLanguage { get; set; }

        /// <summary>
        /// Gets or sets a pattern to be used to display the title, where the title replaces {0}
        /// </summary>
        /// <example>Standard text before the title {0} Standard text after the title</example>
        public string TitlePattern { get; set; }

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
        /// Gets or sets the BS7666 address of the place the page is about.
        /// </summary>
        /// <value>The BS7666 address.</value>
        public BS7666Address BS7666Address { get; set; }

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
    }
}
