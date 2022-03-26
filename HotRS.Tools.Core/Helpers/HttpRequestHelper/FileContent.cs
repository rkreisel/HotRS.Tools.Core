namespace HotRS.Tools.Core.Helpers.HttpRequestHelper;

/// <summary>
/// Part of the helper class for the file uploading
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FileContent : MultipartFormDataContent
{
    /// <summary>
    /// Combines multiple parts of a file upload into a single object
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="apiParamName"></param>
    public FileContent(string filePath, string apiParamName)
    {
        filePath.CheckForNull<ArgumentNullException>(nameof(filePath), Resources.ARGUEMENTNOTPROVIDED);
        apiParamName.CheckForNull<ArgumentNullException>(nameof(apiParamName), Resources.ARGUEMENTNOTPROVIDED);
        var filestream = File.Open(filePath, FileMode.Open);
        var filename = Path.GetFileName(filePath);

        using var sc = new StreamContent(filestream);
        Add(sc, apiParamName, filename);
    }
}
