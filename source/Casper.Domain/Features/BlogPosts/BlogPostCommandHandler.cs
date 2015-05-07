using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.BlogPosts.Events;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.BlogPosts
{
    public class BlogPostCommandHandler : MarkdownDocumentCommandHandler<BlogPost, PublishBlogPost, PublishedBlogPost>
    {
        public BlogPostCommandHandler(IBlogPostRepository blogPostRepository)
            : base(blogPostRepository, publishBlogPost => new BlogPost(publishBlogPost), publishBlogPost => new PublishedBlogPost(publishBlogPost))
        {
        }
    }
}