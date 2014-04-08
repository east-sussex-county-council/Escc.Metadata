using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace EsccWebTeam.Egms
{
    /// <summary>
    /// Renders a &lt;script /&gt; element to import an external JavaScript
    /// </summary>
    [Obsolete("Use EsccWebTeam.Egms.Script instead as it allows static files to be combined and cached for performance.")]
    public class ScriptImport : Literal
    {
        #region Fields

        private string scriptKey;
        private NameValueCollection config;
        private string tagStub;
        private string tagEnd;
        private string sslStub;
        private bool isSslRequest;
        private string scriptUrl;

        #endregion

        #region Properties

        /// <summary>
        /// The JavaScript file to import
        /// </summary>
        public string Script
        {
            get
            {
                return this.scriptKey;
            }
            set
            {
                this.scriptKey = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptImport"/> class.
        /// </summary>
        public ScriptImport()
        {
            this.config = ConfigurationManager.GetSection("EsccWebTeam.Egms/ScriptFiles") as NameValueCollection;
            if (this.config == null) this.config = ConfigurationManager.GetSection("EsccWebTeam.EgmsWebMetadata/ScriptFiles") as NameValueCollection; // try older syntax
            if (this.config == null)
            {
                throw new ConfigurationErrorsException(EsccWebTeam.Egms.Properties.Resources.ErrorScriptConfig);
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

            this.tagStub = "<script type=\"text/javascript\" src=\"";
            this.tagEnd = "\"></script>";

            // if SSL, load JavaScript using SSL too to avoid warning "this page contains secure and non-secure items"
            this.isSslRequest = (this.Context.Request.Url.Scheme == Uri.UriSchemeHttps);
            if (this.isSslRequest)
            {
                this.sslStub = String.Format(CultureInfo.CurrentCulture, "{0}://{1}", this.Context.Request.Url.Scheme, this.Context.Request.Url.Host);
            }

            StringBuilder sb = new StringBuilder();

            // Load main JavaScript
            if (this.config[this.scriptKey] != null)
            {
                this.scriptUrl = this.config[this.scriptKey];
                sb.Append(this.tagStub);
                if (this.isSslRequest && this.scriptUrl.StartsWith("/")) sb.Append(this.sslStub);
                sb.Append(this.scriptUrl).Append(this.tagEnd).Append(Environment.NewLine);
            }

            this.Text = sb.ToString();
        }

        #endregion

    }
}
