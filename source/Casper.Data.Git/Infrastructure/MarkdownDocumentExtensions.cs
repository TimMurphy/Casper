using Casper.Data.Git.Infrastructure.Metadata;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Data.Git.Infrastructure
{
    internal static class MarkdownDocumentExtensions
    {
        internal static string Serialize(this MarkdownDocument markdownDocument, IYamlMarkdown yamlMarkdown)
        {
            return yamlMarkdown.Serialize(new MarkdownMetadata(markdownDocument), markdownDocument.Content);
        }
    }
}
