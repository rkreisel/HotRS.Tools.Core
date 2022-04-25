namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Extensions to the string class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns a DateTime? with a value if TryParse is successful, otherwise a null DateTime?
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static DateTime? ToNullableDateTime(this string s) => DateTime.TryParse(s, out var parsedDate) ? parsedDate as DateTime? : null;

    /// <summary>
    /// Returns a int? with a value if TryParse is successful, otherwise a null int?
    /// </summary>
    /// <param name="s"></param>
    public static int? ToNullableInt(this string s) => int.TryParse(s, out var i) ? i as int? : null;

    /// <summary>
    /// Escapes the pipe character by prepending a backslash.
    /// Useful for Excel file creation.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string AddCSVInjectionProtection(this string source)
    {
        return source?.Replace("|", @"\|", StringComparison.InvariantCulture).Replace(@"\\", @"\", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Removes the CSV injection protection added by AddCSVInjectionProtection
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string RemoveCSVInjectionProtection(this string source) => source.Replace(@"\|", "|", StringComparison.InvariantCulture);

    /// <summary>
    /// Parses a string from an Excel format date string
    /// </summary>
    /// <param name="source"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string DateStringFromExcelDateString(this string source, string format = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }

        format ??= CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;


        if (!DateTime.TryParse(source, out var resultDate))
        {
            resultDate = DateTime.FromOADate(double.Parse(source, CultureInfo.CurrentCulture));
        }
        return resultDate != DateTime.MinValue ? resultDate.ToString(format, CultureInfo.CurrentCulture) : null;
    }

    /// <summary>
    /// Appends the values in a List to a string with the given prefix
    /// </summary>
    /// <typeparam name="T">A</typeparam>
    /// <param name="source">A string</param>
    /// <param name="list">A List of objects. The embedded ToString() method will be called to determine the string to append.</param>
    /// <param name="prefix">The string to prepend to the strings extracted from the list. (This is string will be used in the string parameter of s String.Join() command.)</param>
    /// <returns>A string with each value of the list formatted and appended to the original string.</returns>
    public static string AppendListToString<T>(this string source, List<T> list, string prefix = ", ")
    {
        prefix ??= string.Empty;

        if (list.IsNullOrEmpty())
        {
            return source;
        }
        return $"{source}{string.Join(prefix, list.ToArray())}";
    }

}
