using Casper.Data.Git.Infrastructure.Metadata;
using Casper.Domain.Features.BlogPosts;

namespace Casper.Data.Git.Infrastructure
{
    internal static class BlogPostExtensions
    {
        internal static string Serialize(this BlogPost blogPost, IYamlMarkdown yamlMarkdown)
        {
            return yamlMarkdown.Serialize(new BlogPostMetadata(blogPost), blogPost.Content);
        }
    }
}
