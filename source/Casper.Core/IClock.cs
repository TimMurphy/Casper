using System;

namespace Casper.Core
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
    }
}