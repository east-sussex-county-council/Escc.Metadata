using System;
using System.Collections;
using eastsussexgovuk.webservices.EgmsWebMetadata;

namespace EsccWebTeam.Egms
{
	/// <summary>
	/// A collection of mappings between two controlled lists of terms from the Electronic Service Delivery (ESD) Standards site at <a href="http://www.esd.org.uk/standards/">http://www.esd.org.uk/standards/</a>.
	/// </summary>
	public class EsdMappingCollection : CollectionBase
	{
        /// <summary>
        /// Access an EsdMapping in the collection by its index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <remarks>Required for webservice serialization</remarks>
        public EsdMapping this[int index]
        {
            get
            {
                return (EsdMapping)this.List[index];
            }
            set
            { this.List[index] = value; }
        }

        /// <summary>
		/// Add a mapping to the collection
		/// </summary>
		/// <param name="item">The <see cref="EsdMapping" /> to add</param>
		/// <returns>The index at which the mapping was added</returns>
		public int Add(EsdMapping item)
		{
			return List.Add(item);
		}
              	
		/// <summary>
		/// Insert a mapping into the collection at the specified index
		/// </summary>
		/// <param name="index">The index at which to insert the mapping</param>
		/// <param name="item">The <see cref="EsdMapping" /> to insert</param>
		public void Insert(int index, EsdMapping item)
		{
			List.Insert(index, item);
		}
            
		/// <summary>
		/// Remove a mapping from the collection
		/// </summary>
		/// <param name="item">The <see cref="EsdMapping" /> to remove</param>
		public void Remove(EsdMapping item)
		{
			List.Remove(item);
		} 
              	
		/// <summary>
		/// Checks whether a mapping is already in the collection
		/// </summary>
		/// <param name="item">The <see cref="EsdMapping" /> to look for</param>
		/// <returns>True if found in the collection; False otherwise</returns>
		public bool Contains(EsdMapping item)
		{
			return List.Contains(item);
		}
              	
		/// <summary>
		/// Gets the numeric index of a mapping's position in the collection
		/// </summary>
		/// <param name="item">The <see cref="EsdMapping" /> to look for</param>
		/// <returns>The index of the specified <see cref="EsdMapping" /> in the collection</returns>
		public int IndexOf(EsdMapping item)
		{
			return List.IndexOf(item);
		}
              	
		/// <summary>
		/// Copies the contents of the collection to an array
		/// </summary>
		/// <param name="array">The one-dimensional array to copy to</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(EsdMapping[] array, int index)
		{
			List.CopyTo(array, index);
		}
            
	}
}
