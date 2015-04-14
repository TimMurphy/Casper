using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.Messaging
{
    // todo: specifications
    public interface ICommandBus
    {
        void RegisterCommandHandler<TCommand>(Func<TCommand, Task<IEnumerable<object>>> commandHandler) where TCommand : class;
        Task SendCommandAsync(object command);
    }
}