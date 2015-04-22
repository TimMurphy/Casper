using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : MarkdownDocumentRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(IMarkdownDocumentRepositorySettings settings, IGitRepository gitRepository, IMarkdownParser markdownParser, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, markdownParser, yamlMarkdown)
        {
        }
    }
}