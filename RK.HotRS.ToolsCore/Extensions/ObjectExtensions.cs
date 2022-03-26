namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Extensions to Object
/// </summary>
public static class ObjectExtensions
{
    [Obsolete("Use the new built-in ArgumentNullException.ThrowIfNull(o, paramName); instead")]
    public static void CheckForNull(this object o, string paramName)
    {
        ArgumentNullException.ThrowIfNull(o, paramName);
    }

    /// <summary>
    /// Checks the parameter for null and allows the developer to throw a custom exception of T with a custom message
    /// </summary>
    /// <typeparam name="T">The type of exception to throw</typeparam>
    /// <param name="o">The parameter to test</param>
    /// <param name="paramName">The name of the parameter</param>
    /// <param name="message">The message to post</param>
    public static void CheckForNull<T>(this object o, string paramName, string message = "") where T : Exception
    {
        if (o == null)
        {
            var formattedMessage = string.IsNullOrWhiteSpace(message) ? paramName : $"{paramName} - {message}";
            throw (T)Activator.CreateInstance(typeof(T), new object[] { $"{formattedMessage}" });
        }
    }
}
