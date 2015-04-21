using System;

namespace Casper.Core
{
    public static class TimeZoneInfoExtensions
    {
        public static DateTimeOffset ConvertTimeFromUtc(this TimeZoneInfo timeZoneInfo, IClock clock)
        {
            return timeZoneInfo.ConvertTimeFromUtc(clock.UtcNow);
        }

        public static DateTimeOffset ConvertTimeFromUtc(this TimeZoneInfo timeZoneInfo, DateTime utcNow)
        {
            var offset = timeZoneInfo.GetUtcOffset(utcNow);
            var dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);

            var dateTimeOffset = new DateTimeOffset(dateTime, offset);

            return dateTimeOffset;
        }
    }
}
