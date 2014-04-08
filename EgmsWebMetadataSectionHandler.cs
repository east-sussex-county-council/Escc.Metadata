using System;
using System.Configuration;
using System.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace EsccWebTeam.Egms
{
    /// <summary>
    /// Read default metadata and settings from web.config into a <see>eastsussexgovuk.webservices.EgmsWebMetadata.EgmsWebMetadataConfig</see> object
    /// </summary>
    internal class EgmsWebMetadataSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EgmsWebMetadataSectionHandler"/> class.
        /// </summary>
        public EgmsWebMetadataSectionHandler()
        {
        }
        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Read default metadata and settings from web.config into a <see>eastsussexgovuk.webservices.EgmsWebMetadata.EgmsWebMetadataConfig</see> object
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// <para>This is thrown when an attribute expecting an enum value did not recognise the value.</para>
        /// <para>The <b>wcagLevel</b> attribute expects a <see cref="EsccWebTeam.Egms.WcagConformance" />.</para>
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <para>This is thrown when an attribute expecting a boolean value did not recognise the value.</para>
        /// <para>The <b>rnibAccessible</b> attribute expects a boolean.</para>
        /// </exception>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            EgmsWebMetadataConfig config = new EgmsWebMetadataConfig();

            // behaviour
            if (section.Attributes["errorMode"] != null) config.ErrorMode = (MetadataErrorMode)Enum.Parse(typeof(MetadataErrorMode), section.Attributes["errorMode"].Value, true);

            // get current domain as default prefix for virtual uris
            string uriPrefix = Uri.UriSchemeHttp + "://" + HttpContext.Current.Request.Url.Host;
            string uri;

            // other metadata
            if (section.Attributes["titlePattern"] != null) config.TitlePattern = section.Attributes["titlePattern"].Value;
            if (section.Attributes["creator"] != null) config.Creator = section.Attributes["creator"].Value;
            if (section.Attributes["contributor"] != null) config.Contributor = section.Attributes["contributor"].Value;
            if (section.Attributes["custodian"] != null) config.Custodian = section.Attributes["custodian"].Value;
            if (section.Attributes["audience"] != null) config.Audience = section.Attributes["audience"].Value;
            if (section.Attributes["language"] != null) config.Language = section.Attributes["language"].Value;
            if (section.Attributes["person"] != null) config.Person = section.Attributes["person"].Value;
            if (section.Attributes["publisher"] != null) config.Publisher = section.Attributes["publisher"].Value;
            if (section.Attributes["replacesUrl"] != null)
            {
                uri = section.Attributes["replacesUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.ReplacesUrl = new Uri(uri);
            }
            if (section.Attributes["isReplacedByUrl"] != null)
            {
                uri = section.Attributes["isReplacedByUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.IsReplacedByUrl = new Uri(uri);
            }
            if (section.Attributes["isPartOfUrl"] != null)
            {
                uri = section.Attributes["isPartOfUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.IsPartOfUrl = new Uri(uri);
            }
            if (section.Attributes["spatialCoverage"] != null) config.SpatialCoverage = section.Attributes["spatialCoverage"].Value;
            if (section.Attributes["temporalCoverage"] != null) config.TemporalCoverage = section.Attributes["temporalCoverage"].Value;
            if (section.Attributes["picsLabel"] != null) config.PicsLabel = section.Attributes["picsLabel"].Value;
            if (section.Attributes["icraUrl"] != null)
            {
                uri = section.Attributes["icraUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.IcraUrl = new Uri(uri);
            }

            // non-string types
            try
            {
                // accessibility
                if (section.Attributes["rnibAccessible"] != null) config.RnibAccessible = Boolean.Parse(section.Attributes["rnibAccessible"].Value);
                if (section.Attributes["wcagLevel"] != null) config.WcagLevel = (WcagConformance)Enum.Parse(typeof(WcagConformance), section.Attributes["wcagLevel"].Value, true);

                if (section.Attributes["foiExempt"] != null) config.FoiExempt = Boolean.Parse(section.Attributes["foiExempt"].Value);
            }
            catch (Exception ex)
            {
                // publisher is much more informative than default .NET error page
                // which just says "Exception in section handler"
                ExceptionManager.Publish(ex);
                throw;
            }

            // dates
            if (section.Attributes["dateCreated"] != null) config.DateCreated = section.Attributes["dateCreated"].Value;
            if (section.Attributes["dateIssued"] != null) config.DateIssued = section.Attributes["dateIssued"].Value;
            if (section.Attributes["dateModified"] != null) config.DateModified = section.Attributes["dateModified"].Value;
            if (section.Attributes["dateNextVersionDue"] != null) config.DateNextVersionDue = section.Attributes["dateNextVersionDue"].Value;
            if (section.Attributes["dateAutoRemove"] != null) config.DateAutoRemove = section.Attributes["dateAutoRemove"].Value;
            if (section.Attributes["dateReview"] != null) config.DateReview = section.Attributes["dateReview"].Value;

            // LAWs controlled vocabularies
            if (section.Attributes["ipsvPreferredTerms"] != null) config.IpsvPreferredTerms = section.Attributes["ipsvPreferredTerms"].Value;
            if (section.Attributes["ipsvNonPreferredTerms"] != null) config.IpsvNonPreferredTerms = section.Attributes["ipsvNonPreferredTerms"].Value;
            if (section.Attributes["educationAndSkillsPreferredTerms"] != null) config.EducationAndSkillsPreferredTerms = section.Attributes["educationAndSkillsPreferredTerms"].Value;
            if (section.Attributes["educationAndSkillsNonPreferredTerms"] != null) config.EducationAndSkillsNonPreferredTerms = section.Attributes["educationAndSkillsNonPreferredTerms"].Value;
            if (section.Attributes["keywords"] != null) config.Keywords = section.Attributes["keywords"].Value;
            if (section.Attributes["lgilType"] != null) config.LgilType = section.Attributes["lgilType"].Value;
            if (section.Attributes["lgtlType"] != null) config.LgtlType = section.Attributes["lgtlType"].Value;
            if (section.Attributes["lgslNumbers"] != null) config.LgslNumbers = section.Attributes["lgslNumbers"].Value;
            if (section.Attributes["lgalTypes"] != null) config.LgalTypes = section.Attributes["lgalTypes"].Value;

            // <link /> elements
            if (section.Attributes["copyrightUrl"] != null)
            {
                uri = section.Attributes["copyrightUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.CopyrightUrl = new Uri(uri);
            }
            if (section.Attributes["helpUrl"] != null)
            {
                uri = section.Attributes["helpUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.HelpUrl = new Uri(uri);
            }
            if (section.Attributes["searchUrl"] != null)
            {
                uri = section.Attributes["searchUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.SearchUrl = new Uri(uri);
            }
            if (section.Attributes["homeUrl"] != null)
            {
                uri = section.Attributes["homeUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.HomeUrl = new Uri(uri);
            }
            if (section.Attributes["helpToolTip"] != null) config.HelpToolTip = section.Attributes["helpToolTip"].Value;
            if (section.Attributes["searchToolTip"] != null) config.SearchToolTip = section.Attributes["searchToolTip"].Value;
            if (section.Attributes["homeToolTip"] != null) config.HomeToolTip = section.Attributes["homeToolTip"].Value;
            if (section.Attributes["openSearchUrl"] != null && section.Attributes["openSearchTitle"] != null)
            {
                uri = section.Attributes["openSearchUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.OpenSearchUrl = new OpenSearchUrl();
                config.OpenSearchUrl.Title = section.Attributes["openSearchTitle"].Value;
                config.OpenSearchUrl.Url = new Uri(uri);
            }

            if (section.Attributes["iconUrl"] != null)
            {
                uri = section.Attributes["iconUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.IconUrl = new Uri(uri);
            }

            if (section.Attributes["hasTouchIcon"] != null) config.HasTouchIcon = Boolean.Parse(section.Attributes["hasTouchIcon"].Value);

            // Facebook open graph protocol
            if (section.Attributes["facebookAppId"] != null) config.FacebookAppId = section.Attributes["facebookAppId"].Value;
            if (section.Attributes["openGraphType"] != null) config.OpenGraphType = section.Attributes["openGraphType"].Value;
            if (section.Attributes["siteImageUrl"] != null)
            {
                uri = section.Attributes["siteImageUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.SiteImageUrl = new Uri(uri);
            }
            if (section.Attributes["siteName"] != null) config.SiteName = section.Attributes["siteName"].Value;

            // Twitter cards
            if (section.Attributes["twitterCardType"] != null) config.TwitterCardType = section.Attributes["twitterCardType"].Value;
            if (section.Attributes["twitterAccount"] != null) config.TwitterAccount = section.Attributes["twitterAccount"].Value;

            // Windows tile icons
            if (section.Attributes["windowsTileColour"] != null) config.WindowsTileColour = section.Attributes["windowsTileColour"].Value;
            if (section.Attributes["windowsTileIconUrl"] != null)
            {
                uri = section.Attributes["windowsTileIconUrl"].Value;
                if (uri.StartsWith("/")) uri = uriPrefix + uri;
                config.WindowsTileIconUrl = new Uri(uri);
            }

            return config;
        }

        #endregion
    }
}
