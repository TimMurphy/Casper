using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.BlogPosts
{
    public interface IBlogPostRepository : IMarkdownDocumentRepository<BlogPost>
    {
    }
}