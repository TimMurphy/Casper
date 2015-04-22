using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.Pages
{
    public interface IPageRepository : IMarkdownDocumentRepository<Page>
    {
    }
}