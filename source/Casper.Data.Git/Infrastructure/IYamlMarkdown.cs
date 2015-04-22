using Casper.Data.Git.Infrastructure.Metadata;

namespace Casper.Data.Git.Infrastructure
{
    public interface IYamlMarkdown
    {
        string Serialize(MarkdownMetadata metadata, string markdown);
        void Deserialize(string markdownWithFrontMatter, out MarkdownMetadata markdownMetadata, out string markdown);
    }
}