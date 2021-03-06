using System;

namespace Escc.Web.Metadata
{
	/// <summary>
	/// Details of an <a href="http://www.opensearch.org/">OpenSearch</a> search engine description
	/// </summary>
	public class OpenSearchUrl
	{

		#region Private variables

		private Uri url;
		private string title = String.Empty;

		#endregion Private variables

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenSearchUrl"/> class.
		/// </summary>
		public OpenSearchUrl()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenSearchUrl"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		public OpenSearchUrl(Uri url)
		{
			this.Url = url;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="OpenSearchUrl"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="title">The title.</param>
		public OpenSearchUrl(Uri url, string title)
		{
			this.Url = url;
			this.Title = title;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		public Uri Url
		{
			get { return this.url; }
			set { this.url = value; }
		}

		#endregion Properties

	}
}