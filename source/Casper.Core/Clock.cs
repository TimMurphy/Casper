using System;

namespace Casper.Core
{
    public class Clock : IClock
    {
        private readonly TimeSpan _offset;

        public Clock(TimeSpan offset)
        {
            _offset = offset;
        }

        /// <summary>
        ///     Gets the current date & time for the current user.
        /// </summary>
        public DateTimeOffset Now
        {
            get
            {
                var now = DateTime.UtcNow.Add(_offset);
                return new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond, _offset);
            }
        }
    }
}