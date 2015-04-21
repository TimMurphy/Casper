using System;

namespace Casper.Core
{
    public static class TimeZoneInfoExtensions
    {
        public static DateTimeOffset ToLocalTime(this TimeZoneInfo timeZoneInfo, IClock clock)
        {
            return TimeZoneInfo.ConvertTime(clock.UtcNow, timeZoneInfo);
        }
    }
}
