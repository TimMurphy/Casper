using System;
using Casper.Core;

namespace Casper.Data.Git.Specifications.Helpers.Dummies
{
    public class DummyClock : IClock
    {
        public DummyClock()
        {
            UtcNowFactory = () => DateTime.UtcNow;
        }

        public DateTime UtcNow => UtcNowFactory();
        public static Func<DateTime> UtcNowFactory { get; set; }
    }
}