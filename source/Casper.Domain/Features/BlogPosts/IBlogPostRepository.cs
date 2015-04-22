using System.Collections.Generic;
using System.Threading.Tasks;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.BlogPosts
{
    public interface IBlogPostRepository : IMarkdownDocumentRepository<BlogPost>
    {
        Task<IEnumerable<BlogPost>> FindPublishedBlogPostsAsync(IPagination pagination);
    }
}