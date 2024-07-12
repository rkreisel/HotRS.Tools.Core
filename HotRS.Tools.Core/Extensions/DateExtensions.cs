namespace HotRS.Tools.Core.Extensions;

/// <summary>
/// Simple Utilities and Extensions for the DateTime class
/// </summary>
public static class DateExtensions
{
    public static int AsUnixSeconds(this DateTime source) =>
      ToUnixTimeSeconds(source);


    public static int ToUnixTimeSeconds(DateTime dateTime)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
        return (int)dateTimeOffset.ToUnixTimeSeconds();
    }

    public static DateTime UnixSecondsAsDateTime(this int unixSeconds) =>
        FromUnixSeconds(unixSeconds);

    public static DateTime FromUnixSeconds(int unixSeconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixSeconds).DateTime;
    }
}
