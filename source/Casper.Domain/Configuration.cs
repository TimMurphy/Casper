using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.Files;
using Casper.Domain.Features.Files.Commands;
using Casper.Domain.Features.Pages;
using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain
{
    public static class Configuration
    {
        public static void Configure(ICommandBus commandBus, IBlogPostRepository blogPostRepository, IPageRepository pageRepository, IFileRepository fileRepository)
        {
            RegisterCommandHandlers(commandBus, blogPostRepository, pageRepository, fileRepository);
        }

        private static void RegisterCommandHandlers(ICommandBus commandBus, IBlogPostRepository blogPostRepository, IPageRepository pageRepository, IFileRepository fileRepository)
        {
            // todo: add auto registration
            commandBus.RegisterCommandHandler<PublishBlogPost>(command => (new BlogPostCommandHandler(blogPostRepository)).HandleAsync(command));
            commandBus.RegisterCommandHandler<PublishPage>(command => (new PageCommandHandler(pageRepository)).HandleAsync(command));
            commandBus.RegisterCommandHandler<UploadFile>(command => (new FileCommandHandler(fileRepository)).HandleAsync(command));
        }
    }
}
