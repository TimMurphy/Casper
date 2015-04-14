using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure;

namespace Casper.Domain.Features.BlogPosts.Commands
{
    public class PublishBlogPost
    {
        public PublishBlogPost(string title, string content, DateTimeOffset published, IAuthor author, string blogDirectory, ISlugFactory slugFactory)
        {
            Title = title;
            Content = content;
            Published = published;
            Author = author;
            Path = string.Format("{0}/{1}/{2:D2}/{3:D2}/{4}.md", blogDirectory, published.Year, published.Month, published.Day, slugFactory.CreateSlug(title));
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset Published { get; private set; }
        public string Path { get; private set; }
        public static IAuthor Author { get; private set; }

        public string GetFileContents()
        {
            return string.Format("# {0}{1}{1}{2}", Title, Environment.NewLine, Content);
        }
    }
}