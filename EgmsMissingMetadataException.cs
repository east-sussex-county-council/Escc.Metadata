using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace EsccWebTeam.Egms
{
	/// <summary>
	/// Exception thrown when metadata required by the e-Government Metadata Standard is missing from a web page
	/// </summary>
	[Serializable]
	internal sealed class EgmsMissingMetadataException : ApplicationException
	{
		private string missingMetadata = "";
		
		/// <summary>
		/// Sets the name of the metadata element which is missing
		/// </summary>
		public void SetMissingMetadataField(string fieldName)
		{
			this.missingMetadata = fieldName;
		}


		/// <summary>
		/// Gets the message, incidicating which field was missing
		/// </summary>
		public override string Message
		{
			get
			{
				StringBuilder sb = new StringBuilder("Metadata required by the e-GMS was missing from the web page: ");
				sb.Append(Environment.NewLine);
				sb.Append(HttpContext.Current.Request.RawUrl);
				sb.Append(".");
				sb.Append(Environment.NewLine);
				if (this.missingMetadata.Length > 0) 
				{
					sb.Append("The missing field is: ");
					sb.Append(this.missingMetadata);
					sb.Append(".");
					sb.Append(Environment.NewLine);
				}
				if (base.Message != "Error in the application.") sb.Append(base.Message);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EgmsMissingMetadataException" /> class with no arguments.
		/// </summary>
		public EgmsMissingMetadataException () : base()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EgmsMissingMetadataException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public EgmsMissingMetadataException (string message) : base (message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EgmsMissingMetadataException" /> class with a specified error message and 
		/// a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <em>innerException</em>
		/// parameter is not a null reference, the current exception is raised in a <strong>catch</strong> block that handles 
		/// the inner exception.</param>
		public EgmsMissingMetadataException (string message, Exception inner) : base (message, inner)
		{
		}


		
		/// <exclude />
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
		public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
		}
		
	}
}
