
namespace Escc.Web.Metadata
{
    /// <summary>
    /// Identifies an object which accepts metadata using a .Metadata property
    /// </summary>
    public interface IHasMetadata
    {
        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        IMetadata Metadata { get; }
    }
}
