using Casper.Domain.Features.BlogPosts;

namespace Casper.Data.Git.Infrastructure.Metadata
{
    internal class BlogPostMetadata
    {
        internal BlogPostMetadata(BlogPost blogPost)
        {
            Title = blogPost.Title;
            Published = blogPost.Published.ToString("o");
            Author = new AuthorMetdata(blogPost.Author);
        }

        public string Title { get; set; }
        public string Published { get; set; }
        public AuthorMetdata Author { get; set; }
    }
}