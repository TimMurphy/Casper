using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Pages;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Repositories
{
    public class PageRepository : MarkdownDocumentRepository<Page>, IPageRepository
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public PageRepository(IPageRepositorySettings settings, IGitRepository gitRepository, IMarkdownParser markdownParser, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, markdownParser, yamlMarkdown)
        {
        }
    }
}