using MNHH = Microsoft.Net.Http.Headers;

namespace HotRS.Tools.Core.Helpers.FileUpload;

/// <summary>    /// MultipartRequestHeader object    /// </summary>    
[ExcludeFromCodeCoverage]
public static class MultipartRequestHelper
{
    /// <summary>        
    /// Returns a string segment        
    /// </summary>
    /// <param name="contentType"></param>        
    /// <param name="lengthLimit"></param>        
    /// <returns></returns>        
    /// 

    // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"        
    // The spec says 70 characters is a reasonable limit.        
    public static string GetBoundary(MNHH.MediaTypeHeaderValue contentType, int lengthLimit)
    {
        if (contentType == null) { throw new ArgumentNullException(nameof(contentType)); }
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).ToString(); if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException(Resources.MISSINGCONTETETYPEBOUNDARY);
        }
        if (boundary.Length > lengthLimit)
        {
            throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
        }
        return boundary;
    }
    /// <summary>        
    /// Determines if the content is a multipart item        
    /// </summary>        
    /// <param name="contentType"></param>        
    /// <returns></returns>        
    public static bool IsMultipartContentType(string contentType) =>
        !string.IsNullOrWhiteSpace(contentType)
        && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);

    /// <summary>        
    /// Determines if the content is form-data        
    /// </summary>        
    /// <param name="contentDisposition"></param>        
    /// <returns></returns>        
    public static bool HasFormDataContentDisposition(MNHH.ContentDispositionHeaderValue contentDisposition) =>
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
    public static bool HasFileContentDisposition(MNHH.ContentDispositionHeaderValue contentDisposition) =>
        // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
        contentDisposition != null
        && contentDisposition.DispositionType.Equals(Resources.FORMDATA, StringComparison.CurrentCulture)
        && (!string.IsNullOrWhiteSpace(contentDisposition.FileName.ToString())
        || !string.IsNullOrWhiteSpace(contentDisposition.FileNameStar.ToString()));
}

