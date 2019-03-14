using System;
using System.Collections.Generic;
using System.Text;

namespace Escc.Metadata
{
    /// <summary>
    /// Details of an image which should be referenced in metadata
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// The absolute URL of the image
        /// </summary>
        public Uri ImageUrl { get; set; }

        /// <summary>
        /// The alternative text for the image
        /// </summary>
        public string AlternativeText { get; set; }
    }
}
