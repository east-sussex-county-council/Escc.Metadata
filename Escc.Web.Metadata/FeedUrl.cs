using System;

namespace Escc.Web.Metadata
{
	/// <summary>
	/// Details of an RSS feed
	/// </summary>
	public class FeedUrl
	{

		#region Private variables

		private Uri url;
		private string rel = "alternative";
		private string title = String.Empty;

		#endregion Private variables

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedUrl"/> class.
		/// </summary>
		public FeedUrl()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedUrl"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		public FeedUrl(Uri url)
		{
			this.Url = url;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedUrl"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="relationship">The relationship.</param>
		public FeedUrl(Uri url, string relationship)
		{
			this.Url = url;
			this.Relationship = relationship;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FeedUrl"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="relationship">The relationship.</param>
		/// <param name="title">The title.</param>
		public FeedUrl(Uri url, string relationship, string title)
		{
			this.Url = url;
			this.Relationship = relationship;
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

		/// <summary>
		/// Gets or sets the relationship.
		/// </summary>
		/// <value>The relationship.</value>
		public string Relationship
		{
			get { return this.rel; }
			set { this.rel = value; }
		}

		#endregion Properties

	}
}