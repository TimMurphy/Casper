using System;
using System.IO;
using Anotar.LibLog;
using Casper.Core;
using Casper.Data.Git.Infrastructure.Metadata;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.MarkdownDocuments;
using OpenMagic.Extensions;

namespace Casper.Data.Git.Infrastructure
{
    public static class MarkdownDocumentSerialization
    {
        public static TDocument Deserialize<TDocument>(
            Func<string, string, string, DateTimeOffset, Author, TDocument> documentFactory,
            string relativeUri,
            string markdownWithFrontMatter,
            IYamlMarkdown yamlMarkdown)
            where TDocument : MarkdownDocument
        {
            MarkdownMetadata metadata;
            string markdown;

            yamlMarkdown.Deserialize(markdownWithFrontMatter, out metadata, out markdown);

            var published = DateTimeOffset.Parse(metadata.Published);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(metadata.Author.TimeZoneId);
            var author = new Author(metadata.Author.Name, metadata.Author.Email, timeZoneInfo);
            var document = documentFactory(relativeUri, metadata.Title, markdown, published, author);

            return document;
        }

        public static TDocument DeserializeFromFile<TDocument>(
            Func<string, string, string, DateTimeOffset, Author, TDocument> documentFactory,
            string file,
            DirectoryInfo publishedDirectory,
            IYamlMarkdown yamlMarkdown)
            where TDocument : MarkdownDocument
        {
            LogTo.Trace("DeserializeFromFile(file: {0}, publishedDirectory: {1}, yamlMarkdown)", file, publishedDirectory);

            var relativeUri = GetRelativeUriFromFile(publishedDirectory, file);
            var markdownWithFrontMatter = File.ReadAllText(file);

            return Deserialize(documentFactory, relativeUri, markdownWithFrontMatter, yamlMarkdown);
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static string GetRelativeUriFromFile(DirectoryInfo publishedDirectory, string blogPostFile)
        {
            return blogPostFile.TextAfter(publishedDirectory.FullName + "\\").ToUnixSlashes().TrimEnd(".md");
        }
    }
}