namespace Hainz.Common.Extensions;

public static class DateTimeExtensions
{
    public static string ToDateTimeString(this DateTime datetime) => datetime.ToString("MM-dd-yyyy HH:mm:ss");
}