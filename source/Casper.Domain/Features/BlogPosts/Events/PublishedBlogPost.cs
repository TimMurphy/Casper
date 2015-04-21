using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Features.BlogPosts.Events
{
    public class PublishedBlogPost : IEvent
    {
        public PublishedBlogPost(PublishBlogPost command)
            : this(command.RelativeUri, command.Title, command.Title, command.Published, command.Author)
        {
        }

        private PublishedBlogPost(string relativeUri, string title, string content, DateTimeOffset published, Author author)
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
