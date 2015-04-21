using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    public class CommandBus : ICommandBus
    {
        private readonly ConcurrentDictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>> _commandHandlers = new ConcurrentDictionary<Type, Func<ICommand, Task<IEnumerable<IEvent>>>>();
        private readonly IEventBus _eventBus;

        public CommandBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void RegisterCommandHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> commandHandler) where TCommand : class, ICommand
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> action = command => commandHandler((TCommand)command);

            if (_commandHandlers.TryAdd(typeof(TCommand), action))
            {
                return;
            }
            throw new DuplicateCommandHandlerException(typeof(TCommand));
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var commandHandler = GetCommandHandler(command);
            var events = await commandHandler(command);

            await _eventBus.PublishEvents(events);
        }

        private Func<ICommand, Task<IEnumerable<IEvent>>> GetCommandHandler(object command)
        {
            Func<ICommand, Task<IEnumerable<IEvent>>> commandHandler;

            if (_commandHandlers.TryGetValue(command.GetType(), out commandHandler))
            {
                return commandHandler;
            }
            throw new CommandHandlerNotFoundException(command.GetType());
        }
    }
}