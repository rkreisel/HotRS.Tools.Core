namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Extensions to the list of ValidationResults returned from Validate()
/// </summary>
public static class ValidationErrorExtensions
{
    /// <summary>
    /// Formats the error messages into a string using the specified delimiter and optional line feed.
    /// </summary>
    /// <param name="source">A list of ValidationResult objects.</param>
    /// <param name="delimiter">The value to be used to separate messages.</param>
    /// <param name="useLineFeed">rue or False</param>
    /// <param name="includeMemberNames">Adds teh values of MemberNames from the error object.</param>
    /// <returns>A formatted string of error messages.</returns>
    public static string FormatErrors(this IList<ValidationResult> source, string delimiter = ", ", bool useLineFeed = false, bool includeMemberNames = false) =>
        string.Join($"{delimiter}{(useLineFeed ? Environment.NewLine : string.Empty)}", source.Select(x => x.ErrorMessage + (includeMemberNames ? ($" (Member Names: {string.Join(delimiter, x.MemberNames.ToArray())})") : string.Empty)));
}
