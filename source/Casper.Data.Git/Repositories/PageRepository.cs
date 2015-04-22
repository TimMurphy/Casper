using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Pages;

namespace Casper.Data.Git.Repositories
{
    public class PageRepository : MarkdownDocumentRepository<Page>, IPageRepository
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public PageRepository(IPageRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, yamlMarkdown)
        {
        }
    }
}