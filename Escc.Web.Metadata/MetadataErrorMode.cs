namespace Escc.Web.Metadata
{
	/// <summary>
	/// Sets whether metadata errors should cause exceptions
	/// </summary>
	public enum MetadataErrorMode
	{
		/// <summary>
		/// Throw exceptions when mandatory metadata is missing
		/// </summary>
		On,

		/// <summary>
		/// Don't throw exceptions when mandatory metadata is missing
		/// </summary>
		Off,

		/// <summary>
		/// Throw exceptions when mandatory metadata is missing and the request is not from local machine
		/// </summary>
		RemoteOnly,

		/// <summary>
		/// Throw exceptions when mandatory metadata is missing and the request is from local machine
		/// </summary>
		LocalOnly
	}
}
