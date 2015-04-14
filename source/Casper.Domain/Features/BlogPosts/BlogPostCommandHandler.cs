using System.Collections.Generic;
using System.Threading.Tasks;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.BlogPosts.Events;

namespace Casper.Domain.Features.BlogPosts
{
    public class BlogPostCommandHandler
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostCommandHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<IEnumerable<object>> HandleAsync(PublishBlogPost command)
        {
            await _blogPostRepository.PublishAsync(command);

            return new[] { new PublishedBlogPost() };
        }
    }
}