namespace HotRS.Tools.Core.Helpers.HttpRequestHelper;

/// <summary>
/// PatchContent class
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class PatchContent : StringContent
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value"></param>
    public PatchContent(object value)
       : base(JsonConvert.SerializeObject(value), Encoding.UTF8,
                 "application/json-patch+json")
    {
    }
}
