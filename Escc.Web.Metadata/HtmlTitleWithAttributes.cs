using System.Web.UI;

namespace Escc.Web.Metadata
{
    /// <summary>
    /// Title element which allows attributes to be used
    /// </summary>
    internal class HtmlTitleWithAttributes : System.Web.UI.HtmlControls.HtmlTitle
    {
        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.HtmlControls.HtmlTitle"/> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that contains the output stream to render on the client.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteBeginTag(this.TagName);
            foreach (string key in this.Attributes.Keys) writer.WriteAttribute(key, this.Attributes[key]);
            writer.Write(HtmlTextWriter.TagRightChar);
            this.RenderChildren(writer);
            writer.WriteEndTag(this.TagName);
        }
    }
}
