using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    public interface IEventBus
    {
        void SubscribeTo<TEvent>(Func<TEvent, Task> eventHandler);
        Task PublishEvents(IEnumerable<object> events);
    }
}