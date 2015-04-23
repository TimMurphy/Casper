using System;
using System.IO;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.MarkdownDocuments.Commands;

namespace Casper.Domain.Infrastructure.MarkdownDocuments
{
    public abstract class MarkdownDocument
    {
        protected MarkdownDocument(PublishMarkdownDocument command)
            : this(command.RelativeUri, command.Title, command.Content, command.Published, command.Author)
        {
        }

        protected MarkdownDocument(string relativeUri, string title, string content, DateTimeOffset published, Author author)
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

        public string Name
        {
            get { return Path.GetFileNameWithoutExtension(RelativeUri); }
        }
    }
}