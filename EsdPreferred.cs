using System;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
	/// <summary>
	/// Terms from controlled lists on the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a> are either preferred or non-preferred.
	/// </summary>
	public enum EsdPreferredState
	{
		/// <summary>
		/// Term is the preferred term for a concept
		/// </summary>
		Preferred,
		
		/// <summary>
		/// Term which describes a concept, but has a preferred synonym
		/// </summary>
		NonPreferred,

		/// <summary>
		/// Use either a preferred or non-preferred term
		/// </summary>
		Any
	}
}
