using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    // todo: specifications
    public interface ICommandBus
    {
        void RegisterCommandHandler<TCommand>(Func<TCommand, Task<IEnumerable<IEvent>>> commandHandler) where TCommand : class, ICommand;
        Task SendCommandAsync(ICommand command);
    }
}