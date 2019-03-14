using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.Metadata
{
    /// <summary>
    /// Metadata unique to Twitter
    /// </summary>
    public class TwitterMetadata
    {
        /// <summary>
        /// Gets or sets Twitter card type.
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
