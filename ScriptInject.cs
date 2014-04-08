using System;
using System.Web.UI;

namespace EsccWebTeam.Egms
{
    /// <summary>
    ///  /// Renders tags to inject script files into the head section of a page
    /// </summary>
    [Obsolete("Use EsccWebTeam.Egms.Script instead as it allows static files to be combined and cached for performance.")]
    public class ScriptInject : ScriptImport
    {
        /// <summary>
        /// Gets or sets the destination placeholder id.
        /// </summary>
        /// <value>The destination placeholder id.</value>
        public string DestinationPlaceholder { get; set; }

        /// <summary>
        /// Create the control
        /// </summary>
        protected override void CreateChildControls()
        {
            if (String.IsNullOrEmpty(DestinationPlaceholder))
            {
                throw new ArgumentException("DestinationPlaceholder property must be set to the name of the ContentPlaceholder to inject the JavaScript into");
            }

            base.CreateChildControls();

            Control destinationControl = null;
            if (Page.Master == null)
            {
                if (Page.Header != null)
                {
                    destinationControl = Page.Header;
                }
                else
                {
                    // Can't find destination control so just place it here
                    return;
                }
            }

            if (destinationControl == null) destinationControl = Page.Master.FindControl(DestinationPlaceholder);

            // Can't find destination control so just place it here
            if (destinationControl == null) return;

            base.Visible = false;
            int count = 0;
            foreach (Control control in destinationControl.Controls)
            {
                LiteralControl resource = control as LiteralControl;

                if (resource != null)
                {

                    if (resource.Text == base.Text)
                    {
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                destinationControl.Controls.Add(new LiteralControl(base.Text));
            }


        }
    }
}
