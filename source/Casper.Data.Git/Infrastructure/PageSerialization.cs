using System;
using System.IO;
using Anotar.LibLog;
using Casper.Domain.Features.Pages;
using NullGuard;

namespace Casper.Data.Git.Infrastructure
{
    public static class PageSerialization
    {
        [return: AllowNull]
        public static Page TryDeserializeFromFile(string file, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            LogTo.Trace("TryDeserializeFromFile(file: {0}, publishedDirectory: {1}, yamlMarkdown)", file, publishedDirectory.FullName);

            Page page;

            try
            {
                page = DeserializeFromFile(file, publishedDirectory, yamlMarkdown);
                LogTo.Debug("Deserialized {0}.", file);
            }
            catch (Exception exception)
            {
                LogTo.DebugException(string.Format("Error while deserializing {0}.\n\n{1}", file, exception), exception);
                page = null;
            }

            return page;
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