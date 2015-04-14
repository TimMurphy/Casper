using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain
{
    public static class Configuration
    {
        public static void Configure(ICommandBus commandBus, IBlogPostRepository blogPostRepository)
        {
            RegisterCommandHandlers(commandBus, blogPostRepository);
        }

        private static void RegisterCommandHandlers(ICommandBus commandBus, IBlogPostRepository blogPostRepository)
        {
            // todo: add auto registration
            commandBus.RegisterCommandHandler<PublishBlogPost>(command => (new BlogPostCommandHandler(blogPostRepository)).HandleAsync(command));
        }
    }
}
