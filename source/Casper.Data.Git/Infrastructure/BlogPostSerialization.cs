using System;
using System.IO;
using Anotar.LibLog;
using Casper.Core;
using Casper.Data.Git.Infrastructure.Metadata;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using OpenMagic.Extensions;

namespace Casper.Data.Git.Infrastructure
{
    public static class BlogPostSerialization
    {
        public static BlogPost Deserialize(string relativeUri, string markdownWithFrontMatter, IYamlMarkdown yamlMarkdown)
        {
            MarkdownMetadata metadata;
            string markdown;

            yamlMarkdown.Deserialize(markdownWithFrontMatter, out metadata, out markdown);

            var published = DateTimeOffset.Parse(metadata.Published);
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(metadata.Author.TimeZoneId);
            var author = new Author(metadata.Author.Name, metadata.Author.Email, timeZoneInfo);
            var blogPost = new BlogPost(relativeUri, metadata.Title, markdown, published, author);

            return blogPost;
        }

        public static BlogPost DeserializeFromFile(string blogPostFile, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            LogTo.Trace("DeserializeFromFile(blogPostFile: {0}, publishedDirectory: {1}, yamlMarkdown)", blogPostFile, publishedDirectory);

            var relativeUri = GetRelativeUriFromFile(publishedDirectory, blogPostFile);
            var markdownWithFrontMatter = File.ReadAllText(blogPostFile);

            return Deserialize(relativeUri, markdownWithFrontMatter, yamlMarkdown);
        }

        private static string GetRelativeUriFromFile(DirectoryInfo publishedDirectory, string blogPostFile)
        {
            return blogPostFile.TextAfter(publishedDirectory.FullName + "\\").ToUnixSlashes().TrimEnd(".md");
        }
    }
}