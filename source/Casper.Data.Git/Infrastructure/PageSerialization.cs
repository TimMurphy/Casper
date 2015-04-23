using System;
using System.IO;
using Casper.Domain.Features.Pages;
using NullGuard;

namespace Casper.Data.Git.Infrastructure
{
    public static class PageSerialization
    {
        [return: AllowNull]
        public static Page TryDeserializeFromFile(string file, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            try
            {
                return DeserializeFromFile(file, publishedDirectory, yamlMarkdown);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static Page DeserializeFromFile(string file, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            return MarkdownDocumentSerialization.DeserializeFromFile(
                (relativeUri, title, content, published, author) => new Page(relativeUri, title, content, published, author),
                file,
                publishedDirectory,
                yamlMarkdown);
        }
    }
}