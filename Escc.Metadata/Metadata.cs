using System;
using System.Collections.Generic;
using Escc.AddressAndPersonalDetails;

namespace Escc.Metadata
{
    /// <summary>
    /// Metadata used on web pages
    /// </summary>
    public class Metadata 
    {
        /// <summary>
        /// Gets or sets the title of the web page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the language the title is written in.
        /// </summary>
        /// <value>The title language.</value>
        public string TitleLanguage { get; set; } = "en-GB";

        /// <summary>
        /// Gets or sets a pattern to be used to display the title, where the title replaces {0}
        /// </summary>
        /// <example>Standard text before the title {0} Standard text after the title</example>
        public string TitlePattern { get; set; }

        /// <summary>
        /// Gets or sets the description of the web page
        /// </summary>
        /// <remarks>
        /// <para>This should be set since since it may be the description which is displayed in search result listings.</para>
        /// <para>Try not to repeat information that could be held in another element, eg title, coverage or subject. The description should 
        /// be short but meaningful (under 30 words). It could include the approach to the subject, the reason it’s produced, groups and 
        /// organisations referred to, events covered, key outcomes, broad policy areas etc.</para>
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date the content was created.
        /// </summary>
        public DateTimeOffset? DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date the content was first published
        /// </summary>
        public DateTimeOffset? DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the date the content was modified
        /// </summary>
        public DateTimeOffset? DateModified { get; set; }

        /// <summary>
        /// Gets or sets the date the web page will automatically be removed from publication.
        /// </summary>
        /// <remarks>
        /// This could be used for any data-driven time-sensitive content, such as an events list.
        /// </remarks>
        public DateTimeOffset? DateExpires { get; set; }

        /// <summary>
        /// Gets or sets the person the page is about.
        /// </summary>
        /// <value>The person the page is about.</value>
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the BS7666 address of the place the page is about.
        /// </summary>
        /// <value>The BS7666 address.</value>
        public BS7666Address BS7666Address { get; set; }

        /// <summary>
        /// Gets or sets URIs from the <a href="https://standards.esd.org.uk/?uri=list%2FenglishAndWelshServices">Local Government Service List (LGSL)</a>.
        /// </summary>
        /// <remarks>
        /// <example>http://id.esd.org.uk/service/1234</example>
        /// </remarks>
        public IList<Uri> LocalGovernmentServices { get; } = new List<Uri>();

        /// <summary>
        /// Gets or sets the display name of the site.
        /// </summary>
        /// <value>The name of the site.</value>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets the base URL of the site's icons, including the favicon and Apple touch icon.
        /// </summary>
        /// <remarks>
        /// <para>A base URL is specified rather than a complete URL as these icons tend to reside together with well-known filenames that it's not advisable to change because some browsers assume them.</para>
        /// </remarks>
        public string SiteImageBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the canonical page URL. 
        /// </summary>
        /// <value>The page URL.</value>
        public Uri CanonicalPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of an image representing the page.
        /// </summary>
        /// <value>The image URL.</value>
        public Uri PageImageUrl { get; set; }

        /// <summary>
        /// Metadata unique to Facebook, including any properties which are by definition unique to Open Graph.
        /// </summary>
        public FacebookMetadata Facebook { get; } = new FacebookMetadata();

        /// <summary>
        /// Metadata unique to Twitter
        /// </summary>
        public TwitterMetadata Twitter { get; } = new TwitterMetadata();
        
        /// <summary>
        /// Gets the <a href="http://www.opensearch.org/">OpenSearch</a> plugin URLs.
        /// </summary>
        /// <value>The OpenSearch plugins' URLs.</value>
        public IList<OpenSearchEngine> OpenSearchEngines { get; } = new List<OpenSearchEngine>();
    }
}
