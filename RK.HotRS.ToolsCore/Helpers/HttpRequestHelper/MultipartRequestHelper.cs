using Microsoft.Net.Http.Headers;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Properties;
using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.HttpRequestHelper
{
	/// <summary>
	/// MultipartRequestHeader object
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class MultipartRequestHelper
	{
		/// <summary>
		/// Returns a string segment
		/// </summary>
		/// <param name="contentType"></param>
		/// <param name="lengthLimit"></param>
		/// <returns></returns>
		// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
		// The spec says 70 characters is a reasonable limit.
		public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException(nameof(contentType));
			}

			var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).ToString();
			if (string.IsNullOrWhiteSpace(boundary))
			{
				throw new RKToolsException(Resources.MISSINGCONTETETYPEBOUNDARY);
			}

			if (boundary.Length > lengthLimit)
			{
				throw new RKToolsException($"Multipart boundary length limit {lengthLimit} exceeded.");
			}

			return boundary;
		}

		/// <summary>
		/// Determines if the content is a multipart item
		/// </summary>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public static bool IsMultipartContentType(string contentType) => !string.IsNullOrWhiteSpace(contentType)
				   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

		/// <summary>
		/// Determines if the content is form-data
		/// </summary>
		/// <param name="contentDisposition"></param>
		/// <returns></returns>
		public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition) =>
			// Content-Disposition: form-data; name="key";
			contentDisposition != null
				   && contentDisposition.DispositionType.Equals(Resources.FORMDATA, StringComparison.CurrentCulture)
				   && string.IsNullOrWhiteSpace(contentDisposition.FileName.ToString())
				   && string.IsNullOrWhiteSpace(contentDisposition.FileNameStar.ToString());

		/// <summary>
		/// Determines if the content is a file.
		/// </summary>
		/// <param name="contentDisposition"></param>
		/// <returns></returns>
		public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition) =>
			// Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
			contentDisposition != null
				   && contentDisposition.DispositionType.Equals(Resources.FORMDATA, StringComparison.CurrentCulture)
				   && (!string.IsNullOrWhiteSpace(contentDisposition.FileName.ToString())
					   || !string.IsNullOrWhiteSpace(contentDisposition.FileNameStar.ToString()));
	}
}