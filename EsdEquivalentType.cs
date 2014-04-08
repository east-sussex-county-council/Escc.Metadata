using System;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
	/// <summary>
	/// Relationship of one term to another
	/// </summary>
	public enum EsdEquivalentType
	{
		/// <summary>
		/// An abbreviated form of a related term
		/// </summary>
		Acronym,

		/// <summary>
		/// A common misspelling of a related term
		/// </summary>
		Misspelling,

		/// <summary>
		/// A term which might be used to search for a related term
		/// </summary>
		SearchTerm,

		/// <summary>
		/// A term with a similar meaning to a related term
		/// </summary>
		Synonym,

		/// <summary>
		/// A term with an unknown relationship with a related term
		/// </summary>
		NotSpecified
	}
}
