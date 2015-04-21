using System;

namespace Casper.Core
{
    public interface IClock
    {
        /// <summary>
        ///     Gets the current UTC date & time.
        /// </summary>
        DateTime UtcNow { get; }
    }
}