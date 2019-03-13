using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.Metadata
{
    /// <summary>
    /// Metadata unique to Facebook, including any properties unique to Open Graph
    /// </summary>
    public class FacebookMetadata
    {
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
    }
}
