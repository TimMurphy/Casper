using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    public class CommandBus : ICommandBus
    {
        private readonly ConcurrentDictionary<Type, Func<object, Task<IEnumerable<object>>>> _commandHandlers = new ConcurrentDictionary<Type, Func<object, Task<IEnumerable<object>>>>();
        private readonly IEventBus _eventBus;

        public CommandBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void RegisterCommandHandler<TCommand>(Func<TCommand, Task<IEnumerable<object>>> commandHandler) where TCommand : class
        {
            Func<object, Task<IEnumerable<object>>> action = command => commandHandler((TCommand)command);

            if (_commandHandlers.TryAdd(typeof(TCommand), action))
            {
                return;
            }
            throw new DuplicateCommandHandlerException(typeof(TCommand));
        }

        public async Task SendCommandAsync(object command)
        {
            var commandHandler = GetCommandHandler(command);
            var events = await commandHandler(command);

            await _eventBus.PublishEvents(events);
        }

        private Func<object, Task<IEnumerable<object>>> GetCommandHandler(object command)
        {
            Func<object, Task<IEnumerable<object>>> commandHandler;

            if (_commandHandlers.TryGetValue(command.GetType(), out commandHandler))
            {
                return commandHandler;
            }
            throw new CommandHandlerNotFoundException(command.GetType());
        }
    }
}