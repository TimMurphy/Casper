using System;

namespace Casper.Domain.Infrastructure.Messaging
{
    public class CommandHandlerNotFoundException : Exception
    {
        internal CommandHandlerNotFoundException(Type commandType)
            : base(string.Format("Cannot find command handler for '{0}' command.", commandType))
        {
        }
    }
}