using System;

namespace Casper.Domain.Infrastructure.Messaging
{
    public class DuplicateCommandHandlerException : Exception
    {
        internal DuplicateCommandHandlerException(Type type)
            : base(string.Format("A command handler for '{0}' command has previously been registered.", type))
        {
        }
    }
}