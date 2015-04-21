using System;

namespace Casper.Core
{
    public class Clock : IClock
    {
        /// <summary>
        ///     Gets the current date & time for the current user.
        /// </summary>
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}