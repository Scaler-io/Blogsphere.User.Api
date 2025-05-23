namespace Blogsphere.User.Application.Helpers;
public static class DateTimeHelper
{
    public static DateTime ConvertUtcToIst(DateTime dateTime)
    {
        if(dateTime.Kind != DateTimeKind.Utc)
        {
            return dateTime;
        }

        TimeZoneInfo.TryFindSystemTimeZoneById("India Standard Time", out TimeZoneInfo? zoneInfo);
        DateTime istTime = TimeZoneInfo.ConvertTime(dateTime, destinationTimeZone: zoneInfo);

        return istTime;
    }
}
