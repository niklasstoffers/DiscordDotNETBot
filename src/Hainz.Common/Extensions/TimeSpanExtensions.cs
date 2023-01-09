namespace Hainz.Common.Extensions;

public static class TimeSpanExtensions
{
    public static string ToTimeString(this TimeSpan timespan) => $"{timespan.Hours:00}:{timespan.Minutes:00}:{timespan.Seconds:00}";
}