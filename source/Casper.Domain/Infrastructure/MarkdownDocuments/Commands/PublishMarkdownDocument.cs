using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Infrastructure.MarkdownDocuments.Commands
{
    public abstract class PublishMarkdownDocument : ICommand
    {
        protected PublishMarkdownDocument(string relativeUri, string title, string content, DateTimeOffset published, Author author)
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