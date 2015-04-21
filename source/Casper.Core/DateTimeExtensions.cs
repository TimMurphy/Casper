using System;

namespace Casper.Core
{
    public static class DateTimeExtensions
    {
        public static string ToFolders(this DateTime value)
        {
            return string.Format("{0:D4}/{1:D2}/{2:D2}", value.Year, value.Month, value.Day);
        }
    }
}
