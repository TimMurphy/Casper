using System.IO;
using Casper.Domain.Features.BlogPosts;

namespace Casper.Data.Git.Infrastructure
{
    public static class BlogPostSerialization
    {
        public static BlogPost Deserialize(string relativeUri, string markdownWithFrontMatter, IYamlMarkdown yamlMarkdown)
        {
            return MarkdownDocumentSerialization.Deserialize(
                (rUri, title, content, published, author) => new BlogPost(rUri, title, content, published, author),
                relativeUri,
                markdownWithFrontMatter,
                yamlMarkdown);
        }

        public static BlogPost DeserializeFromFile(string blogPostFile, DirectoryInfo publishedDirectory, IYamlMarkdown yamlMarkdown)
        {
            return MarkdownDocumentSerialization.DeserializeFromFile(
                (relativeUri, title, content, published, author) => new BlogPost(relativeUri, title, content, published, author),
                blogPostFile,
                publishedDirectory,
                yamlMarkdown);
        }
    }
}