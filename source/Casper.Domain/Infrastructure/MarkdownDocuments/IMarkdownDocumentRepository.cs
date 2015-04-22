using System.Threading.Tasks;

namespace Casper.Domain.Infrastructure.MarkdownDocuments
{
    public interface IMarkdownDocumentRepository<in TDocument>
        where TDocument : MarkdownDocument
    {
        Task PublishAsync(TDocument markdownDocument);
    }
}