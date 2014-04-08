namespace EsccWebTeam.Egms
{
	/// <summary>
	/// Levels of conformance to the Web Content Accessibility Guidelines 1.0 at http://www.w3.org/TR/1999/WAI-WEBCONTENT-19990505/ and
    /// the Web Content Accessibility Guidelines 2.0 at http://www.w3.org/TR/WCAG20/
	/// </summary>
	public enum WcagConformance
	{
		/// <summary>
		/// Resource's conformance level has not been specified
		/// </summary>
		Unknown,
		
		/// <summary>
		/// Resource does not conform to any WCAG standard
		/// </summary>
		None,

		/// <summary>
		/// Resource conforms to WCAG 1.0 at Level A
		/// </summary>
		A,

		/// <summary>
		/// Resource conforms to WCAG 1.0 at Level Double-A
		/// </summary>
		AA,

		/// <summary>
		/// Resource conforms to WCAG 1.0 at Level Triple-A
		/// </summary>
		AAA,

        /// <summary>
        /// Resource confirms to WCAG 2.0 at Level A
        /// </summary>
        Wcag2A,

        /// <summary>
        /// Resource conforms to WCAG 2.0 at Level Double-A
        /// </summary>
        Wcag2AA,

        /// <summary>
        /// Resource conforms to WCAG 2.0 at Level Triple-A
        /// </summary>
        Wcag2AAA
	}
}