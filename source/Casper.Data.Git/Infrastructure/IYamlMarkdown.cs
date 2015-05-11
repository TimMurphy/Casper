using Casper.Data.Git.Infrastructure.Metadata;

namespace Casper.Data.Git.Infrastructure
{
    public interface IYamlMarkdown
    {
        void Deserialize(string markdownWithFrontMatter, out MarkdownMetadata markdownMetadata, out string markdown);
        string Serialize(MarkdownMetadata metadata, string markdown);
    }
}