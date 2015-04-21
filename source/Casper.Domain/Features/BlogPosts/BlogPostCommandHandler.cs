using System.Collections.Generic;
using System.Threading.Tasks;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.BlogPosts.Events;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Features.BlogPosts
{
    public class BlogPostCommandHandler
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostCommandHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(PublishBlogPost command)
        {
            var blogPost = new BlogPost(command);

            await _blogPostRepository.PublishAsync(blogPost);

            return new[] { new PublishedBlogPost(command) };
        }
    }
}