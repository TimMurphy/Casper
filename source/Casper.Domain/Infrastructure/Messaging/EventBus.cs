using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _eventHandlers = new ConcurrentDictionary<Type, List<Func<object, Task>>>();

        public void SubscribeTo<TEvent>(Func<TEvent, Task> eventHandler)
        {
            var eventHandlers = FindEventHandlers(typeof (TEvent));
            Func<object, Task> action = @event => @eventHandler((TEvent) @event);

            eventHandlers.Add(action);
        }

        public Task PublishEvents(IEnumerable<object> events)
        {
            return Task.WhenAll(
                from @event in events
                let eventHandlers = FindEventHandlers(@event.GetType())
                from eventHandler in eventHandlers
                select eventHandler(@event));
        }

        private List<Func<object, Task>> FindEventHandlers(Type eventType)
        {
            return _eventHandlers.GetOrAdd(eventType, new List<Func<object, Task>>());
        }
    }
}