using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
    /// <summary>
    /// Renders a &lt;link /&gt; element to import an external stylesheet
    /// </summary>
    [Obsolete("Use EsccWebTeam.Egms.Css instead as it allows static files to be combined and cached for performance.")]
    public class CssImport : Literal
    {
        #region Fields

        private string cssFile;
        private NameValueCollection config;
        private string tagStub;
        private string tagEnd;
        private string tagEndPrint;
        private string sslStub;
        private string cssKey;
        private bool isSslRequest;
        private string cssUrl;
        private NameValueCollection browserTests;

        #endregion

        #region Properties

        /// <summary>
        /// The CSS file to import
        /// </summary>
        public string Css
        {
            get
            {
                return this.cssFile;
            }
            set
            {
                this.cssFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the default value of the media attribute.
        /// </summary>
        /// <value>The default media.</value>
        public string Media { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Renders a &lt;link /&gt; element to import an external stylesheet
        /// </summary>
        public CssImport()
        {
            this.Media = "all";

            // Try progressively older versions of the tag
            this.config = ConfigurationManager.GetSection("EsccWebTeam.Egms/CssFiles") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EgmsWebMetadata/CssFiles") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("cssFiles") as NameValueCollection;
            if (this.config == null)
            {
                throw new ConfigurationErrorsException(EsccWebTeam.Egms.Properties.Resources.ErrorCssConfig);
            }
        }

        #endregion

        #region Permission checks

        /// <summary>
        /// Gets or sets the AD groups to load this CSS file for
        /// </summary>
        /// <value>AD groups names separated by semi-colons</value>
        public string RestrictToGroups { get; set; }

        /// <summary>
        /// Check whether the current user is in the AD group(s) this CSS file is restricted to, if it is restricted
        /// </summary>
        /// <returns></returns>
        private bool PermissionToShow()
        {
            if (String.IsNullOrEmpty(this.RestrictToGroups)) return true;

            try
            {
                // Get a list of the current user's groups. Can't use WindowsPrincipal.IsIsRole() because it doesn't
                // work when web.config is set to use forms authentication. This does work.
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                IdentityReferenceCollection groups = identity.Groups.Translate(typeof(NTAccount));
                List<string> userRoles = new List<string>();
                foreach (IdentityReference group in groups) userRoles.Add(group.Value);

                // See whether any one of the allowed groups is in the user's list of groups
                string[] allowedGroups = this.RestrictToGroups.Split(';');
                foreach (string groupName in allowedGroups)
                {
                    if (userRoles.Contains(groupName.Trim())) return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                return false;
            }
            return false;
        }

        #endregion // Permission checks

        #region Methods

        /// <summary>
        /// Build the &gt;link /&lt; element 
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!this.PermissionToShow()) return;

            this.tagStub = "<link type=\"text/css\" rel=\"stylesheet\" href=\"";
            this.tagEnd = "\" media=\"" + Media + "\" />";
            this.tagEndPrint = "\" media=\"print\" />";
            this.cssKey = this.cssFile;

            // if SSL, load CSS using SSL too to avoid warning "this page contains secure and non-secure items"
            this.isSslRequest = (this.Context.Request.Url.Scheme == Uri.UriSchemeHttps);
            if (this.isSslRequest)
            {
                this.sslStub = String.Format(CultureInfo.CurrentCulture, "{0}://{1}", this.Context.Request.Url.Scheme, this.Context.Request.Url.Host);
            }

            StringBuilder sb = new StringBuilder();

            // Load main CSS
            if (this.config[this.cssKey] != null)
            {
                this.cssUrl = this.config[this.cssKey];
                sb.Append(this.tagStub);
                if (this.isSslRequest && this.cssUrl.StartsWith("/")) sb.Append(this.sslStub);
                sb.Append(this.cssUrl).Append(this.tagEnd).Append(Environment.NewLine);
            }

            // Load print CSS
            string printKey = String.Format(CultureInfo.InvariantCulture, "{0}-Print", this.cssKey);
            if (this.config[printKey] != null)
            {
                this.cssUrl = this.config[printKey];
                sb.Append(this.tagStub);
                if (this.isSslRequest && this.cssUrl.StartsWith("/")) sb.Append(this.sslStub);
                sb.Append(this.cssUrl).Append(this.tagEndPrint).Append(Environment.NewLine);
            }

            // Load IE-specific CSS
            this.CreateBrowserTests();
            foreach (string cssSuffix in this.browserTests.AllKeys)
            {
                this.AddIEFixesCss(sb, cssSuffix);
            }

            this.Text = sb.ToString();
        }

        /// <summary>
        /// Sets up the relationship between browser suffixes in web.config keys and equivalent test expressions in conditional comments
        /// </summary>
        private void CreateBrowserTests()
        {
            this.browserTests = new NameValueCollection();
            this.browserTests.Add("IE", "lte IE 7"); // Legacy - using unqualified "IE" is not future proof. IE7 was current at the time this was used.
            this.browserTests.Add("IE5x", "IE 5");
            this.browserTests.Add("IE50", "IE 5.0");
            this.browserTests.Add("IE55", "IE 5.5");
            this.browserTests.Add("IE6", "IE 6");
            this.browserTests.Add("IE7", "IE 7");
            this.browserTests.Add("IE8", "IE 8");
            this.browserTests.Add("IElte6", "lte IE 6");
            this.browserTests.Add("IElte7", "lte IE 7");
            this.browserTests.Add("IElte8", "lte IE 8");
        }

        /// <summary>
        /// Checks for an IE-specific stylesheet, and adds it using conditional comments
        /// </summary>
        /// <param name="sb">XHTML being built up by the control</param>
        /// <param name="browserKey">Key for specific IE version</param>
        private void AddIEFixesCss(StringBuilder sb, string browserKey)
        {
            string browserCssKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", this.cssKey, browserKey);
            string printCssKey = String.Format(CultureInfo.InvariantCulture, "{0}-Print-{1}", this.cssKey, browserKey);

            // Load main CSS for browser
            if (this.config[browserCssKey] != null)
            {
                this.cssUrl = this.config[browserCssKey];
                sb.Append("<!--[if ").Append(this.browserTests[browserKey]).Append("]>").Append(tagStub);
                if (this.isSslRequest && cssUrl.StartsWith("/")) sb.Append(sslStub);
                sb.Append(this.cssUrl).Append(this.tagEnd).Append("<![endif]-->").Append(Environment.NewLine);
            }

            // Load print CSS for browser
            if (this.config[printCssKey] != null)
            {
                this.cssUrl = this.config[printCssKey];
                sb.Append("<!--[if ").Append(this.browserTests[browserKey]).Append("]>").Append(tagStub);
                if (this.isSslRequest && cssUrl.StartsWith("/")) sb.Append(sslStub);
                sb.Append(this.cssUrl).Append(this.tagEndPrint).Append("<![endif]-->").Append(Environment.NewLine);
            }
        }

        #endregion

    }
}
