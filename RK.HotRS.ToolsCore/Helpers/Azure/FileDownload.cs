using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace RK.HotRS.ToolsCore.Helpers.Azure
{
	/// <summary>
	/// POCO object describing a file to download
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class FileDownload
	{
		/// <summary>
		/// The file content as a MemoryStream
		/// </summary>
		public MemoryStream MemoryStream { get; set; }
		/// <summary>
		/// The THML mime type
		/// </summary>
		public string MimeType { get; set; }
		/// <summary>
		/// The name of the file
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// The exension of the file
		/// </summary>
		public string FileExtension { get; set; }
		/// <summary>
		/// A calculated method to return the full filename
		/// </summary>
		public string FullFileName => $"{this.FileName}{this.FileExtension}";
	}
}
