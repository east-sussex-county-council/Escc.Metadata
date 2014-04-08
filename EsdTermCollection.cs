using System;
using System.Collections;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace eastsussexgovuk.webservices.EgmsWebMetadata
{
    /// <summary>
    /// A collection of terms from a controlled list on the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
    /// </summary>
    public class EsdTermCollection : CollectionBase
    {
        /// <summary>
        /// Add a term to the collection
        /// </summary>
        /// <param name="termName">The text name of the term to add</param>
        /// <returns></returns>
        public int Add(string termName)
        {
            if (termName == null) throw new ArgumentNullException("termName");

            EsdTerm term = new EsdTerm();
            term.Text = termName.Trim();
            return List.Add(term);
        }

        /// <summary>
        /// Add a term to the collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Add(EsdTerm item)
        {
            return List.Add(item);
        }

        /// <summary>
        /// Insert a term into the collection at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, EsdTerm item)
        {
            List.Insert(index, item);
        }

        /// <summary>
        /// Remove a term from the collection
        /// </summary>
        /// <param name="item"></param>
        public void Remove(EsdTerm item)
        {
            List.Remove(item);
        }

        /// <summary>
        /// Checks whether a term is already in the collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(EsdTerm item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Gets the numeric index of a term's position in the collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(EsdTerm item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Gets the numeric index of a term's position in the collection
        /// </summary>
        /// <param name="termId">The term id.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public int IndexOfId(string termId, EsdPreferredState state)
        {
            foreach (EsdTerm term in this)
            {
                if (term.Id == termId
                    && (state == EsdPreferredState.Any || ((state == EsdPreferredState.Preferred) == term.Preferred)))
                {
                    return IndexOf(term);
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the numeric index of a term's position in the collection
        /// </summary>
        /// <param name="termName">Name of the term.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public int IndexOf(string termName, EsdPreferredState state)
        {
            foreach (EsdTerm term in this)
            {
                if (term.Text.ToLowerInvariant() == termName.ToLowerInvariant()
                    && (state == EsdPreferredState.Any || ((state == EsdPreferredState.Preferred) == term.Preferred)))
                {
                    return IndexOf(term);
                }
            }
            return -1;
        }

        /// <summary>
        /// Copies the contents of the collection to an array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(EsdTerm[] array, int index)
        {
            List.CopyTo(array, index);
        }

        /// <summary>
        /// Indexer for the collection
        /// </summary>
        public EsdTerm this[int index]
        {
            get { return (EsdTerm)List[index]; }
            set { List[index] = value; }
        }

        /// <summary>
        /// Gets a semi-colon separated list of the terms
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (EsdTerm term in this)
            {
                if (sb.Length > 0) sb.Append("; ");
                sb.Append(term.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Appends a semi-colon separated list of the terms to the supplied StringBuilder
        /// </summary>
        /// <param name="sb"></param>
        public void AppendText(StringBuilder sb)
        {
            if (this.Count > 0)
            {
                // Add existing terms to ArrayList
                string[] terms = sb.ToString().Split(';');
                for (short i = 0; i < terms.Length; i++) terms[i] = terms[i].Trim();
                ArrayList termsList = new ArrayList(terms);

                // Add terms from this collection, unless already present
                foreach (EsdTerm term in this)
                {
                    if (!termsList.Contains(term.Text))
                    {
                        if (sb.Length > 0) sb.Append("; ");
                        sb.Append(term.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Appends a semi-colon separated list of the term ids to the supplied StringBuilder
        /// </summary>
        /// <param name="sb"></param>
        public void AppendIds(StringBuilder sb)
        {
            if (this.Count > 0)
            {
                // Add existing ids to ArrayList
                string[] terms = sb.ToString().Split(';');
                for (short i = 0; i < terms.Length; i++) terms[i] = terms[i].Trim();
                ArrayList termsList = new ArrayList(terms);

                // Add ids from this collection, unless already present
                foreach (EsdTerm term in this)
                {
                    if (!termsList.Contains(term.Id))
                    {
                        if (sb.Length > 0) sb.Append("; ");
                        sb.Append(term.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Appends a semi-colon separated list of the term ids and their terms to the supplied StringBuilder
        /// </summary>
        /// <param name="sb"></param>
        public void AppendIdsAndText(StringBuilder sb)
        {
            if (this.Count > 0)
            {
                // Add existing values to ArrayList
                string[] terms = sb.ToString().Split(';');
                for (short i = 0; i < terms.Length; i++) terms[i] = terms[i].Trim();
                List<string> termsList = new List<string>(terms);

                // Add terms from this collection, unless already present.
                // The E-GMS 3.1 documentation shows this format with the id before the term (in the LGSL example for Subject),
                // but it doesn't say they have to be that way around. Makes more sense to put the term first so that the value
                // can be sorted.
                foreach (EsdTerm term in this)
                {
                    if (!termsList.Contains(term.Id))
                    {
                        if (sb.Length > 0) sb.Append("; ");
                        sb.Append(term.Text + "; " + term.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Reads a string of semi-colon separated terms into the collection
        /// </summary>
        /// <param name="terms"></param>
        public void ReadString(string terms)
        {
            if (terms != null && terms.Length > 0)
            {
                string[] splitTerms = terms.Split(';');
                foreach (string splitTerm in splitTerms)
                {
                    if (splitTerm.Trim().Length > 0) this.Add(splitTerm);
                }
            }
        }

        /// <summary>
        /// Sorts the collection by the text of the terms
        /// </summary>
        public void Sort()
        {
            this.InnerList.Sort();
        }

    }
}
