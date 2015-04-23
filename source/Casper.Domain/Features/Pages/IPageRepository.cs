using System.Collections.Generic;
using System.Threading.Tasks;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.Pages
{
    public interface IPageRepository : IMarkdownDocumentRepository<Page>
    {
        Task<IEnumerable<Directory>> FindPublishedDirectoriesAsync(string relativeDirectory);
        Task<IEnumerable<Page>> FindPublishedPagesAsync(string relativeDirectory);
    }
}