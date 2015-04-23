using System.IO;
using Casper.Domain.Features.Pages;

namespace Casper.Data.Git.Infrastructure
{
    public static class PageSerialization
    {
        public static Page DeserializeFromFile(string blogPostFile, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            return MarkdownDocumentSerialization.DeserializeFromFile(
                (relativeUri, title, content, published, author) => new Page(relativeUri, title, content, published, author),
                blogPostFile,
                publishedDirectory,
                yamlMarkdown);
        }
    }
}