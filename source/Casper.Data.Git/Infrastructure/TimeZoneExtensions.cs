using System;
using System.Linq;

namespace Casper.Data.Git.Infrastructure
{
    internal static class TimeZoneExtensions
    {
        internal static string GetTimeZoneId(this TimeZone timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.GetSystemTimeZones().Single(t => t.DaylightName == timeZone.DaylightName && t.StandardName == timeZone.StandardName);
            var id = timeZoneInfo.Id;

            return id;
        }
    }
}
