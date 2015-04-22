using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.BlogPosts;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : MarkdownDocumentRepository<BlogPost>, IBlogPostRepository
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public BlogPostRepository(IBlogPostRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, yamlMarkdown)
        {
        }
    }
}