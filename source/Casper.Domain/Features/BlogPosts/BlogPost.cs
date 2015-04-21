using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts.Commands;

namespace Casper.Domain.Features.BlogPosts
{
    public class BlogPost
    {
        public BlogPost(PublishBlogPost command)
            : this(command.RelativeUri, command.Title, command.Content, command.Published, command.Author)
        {
        }

        public BlogPost(string relativeUri, string title, string content, DateTimeOffset published, Author author)
        {
            RelativeUri = relativeUri;
            Title = title;
            Content = content;
            Published = published;
            Author = author;
        }

        public string RelativeUri { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset Published { get; private set; }
        public Author Author { get; private set; }
    }
}