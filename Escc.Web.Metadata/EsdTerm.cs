using System;
using System.Collections.Generic;
using Escc.Web.Metadata.Properties;

namespace Escc.Web.Metadata
{
	/// <summary>
	/// A term from a controlled list on the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
	/// </summary>
	public class EsdTerm : IComparable
	{
		private string id;
		private string scopeNotes;
		private string text;
		private bool preferred;
		private EsdTermCollection relatedTerms;
		private EsdTermCollection broaderTerms;
		private EsdTermCollection childTerms;
		private EsdControlledList controlledList;
		private EsdTermCollection nonPreferredTerms;
		private string historyNotes;
		private bool obsolete;
		private string conceptId;
		private float addedInVersion;
		private float updatedInVersion;
		private bool atoZ;
		private bool category;
		private EsdEquivalentType equivalentType;
		
		/// <summary>
		/// Gets or sets the relationship of this term to its preferred term
		/// </summary>
		public EsdEquivalentType EquivalentType
		{
			get
			{
				return this.equivalentType;
			}
			set
			{
				this.equivalentType = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether the term is a category
		/// </summary>
		public bool Category
		{
			get
			{
				return this.category;
			}
			set
			{
				this.category = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether the term is recommended for use in an A-Z
		/// </summary>
		public bool AtoZ
		{
			get
			{
				return this.atoZ;
			}
			set
			{
				this.atoZ = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the most recent version of the controlled list in which this term was changed
		/// </summary>
		public float UpdatedInVersion
		{
			get
			{
				return this.updatedInVersion;
			}
			set
			{
				this.updatedInVersion = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the version of the controlled list in which this term first appeared
		/// </summary>
		public float AddedInVersion
		{
			get
			{
				return this.addedInVersion;
			}
			set
			{
				this.addedInVersion = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the concept id - the id of the preferred term
		/// </summary>
		public string ConceptId
		{
			get
			{
				return this.conceptId;
			}
			set
			{
				this.conceptId = value;
			}
		}
		
		/// <summary>
		/// Gets or sets whether the term is obsolete
		/// </summary>
		public bool Obsolete
		{
			get
			{
				return this.obsolete;
			}
			set
			{
				this.obsolete = value;
			}
		}
		
		/// <summary>
		/// Gets or sets notes about the history of the term in the hierarchy
		/// </summary>
		public string HistoryNotes
		{
			get
			{
				return this.historyNotes;
			}
			set
			{
				this.historyNotes = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the non-preferred terms related to the current term
		/// </summary>
        public EsdTermCollection NonPreferredTerms
		{
			get
			{
				return this.nonPreferredTerms;
			}
		}
		
		/// <summary>
		/// Gets or sets the controlled list to which the term belongs
		/// </summary>
		public EsdControlledList ControlledList
		{
			get
			{
				return this.controlledList;
			}
			set
			{
				this.controlledList = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the child term(s) in the list hierarchy
		/// </summary>
        public EsdTermCollection ChildTerms
		{
			get
			{
				return this.childTerms;
			}
		}
		
		/// <summary>
		/// Gets or sets the broader term(s) in the list hierarchy
		/// </summary>
        public EsdTermCollection BroaderTerms
		{
			get
			{
				return this.broaderTerms;
			}
		}
		
		/// <summary>
		/// Gets or sets the terms related to this term
		/// </summary>
        public EsdTermCollection RelatedTerms
		{
			get
			{
				return this.relatedTerms;
			}
		}
		
		/// <summary>
		/// Gets or sets whether the term is a preferred term
		/// </summary>
		public bool Preferred
		{
			get
			{
				return this.preferred;
			}
			set
			{
				this.preferred = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the text of the term
		/// </summary>
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}
		
		/// <summary>
		/// Gets or sets an explanation of the term
		/// </summary>
		public string ScopeNotes
		{
			get
			{
				return this.scopeNotes;
			}
			set
			{
				this.scopeNotes = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the official id of the term
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>
		/// Creates a term from a controlled list on the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
		/// </summary>
		public EsdTerm()
		{
            this.nonPreferredTerms = new EsdTermCollection();
            this.relatedTerms = new EsdTermCollection();
            this.broaderTerms = new EsdTermCollection();
            this.childTerms = new EsdTermCollection();
			this.equivalentType = EsdEquivalentType.NotSpecified;
		}

		/// <summary>
		/// Gets the text of the term
		/// </summary>
		public override string ToString()
		{
			return this.Text;
		}

		#region IComparable Members

		/// <summary>
		/// Compares the name of this term with the name of a specified EsdTerm
		/// </summary>
		/// <param name="obj">Another EsdTerm</param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			EsdTerm term = obj as EsdTerm;
			if (term != null)
			{
				return this.Text.CompareTo(term.Text);
			}
			else
			{
                throw new ArgumentException(Resources.ErrorTermExpected, "obj");
			}
		}

		#endregion
	}
}
