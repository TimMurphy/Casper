using System;
using Casper.Core;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class StaticClock : IClock
    {
        public StaticClock()
            : this(DateTime.Now)
        {
        }

        public StaticClock(DateTime utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTime UtcNow { get; private set; }
    }
}