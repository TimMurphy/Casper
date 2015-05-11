using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.Files;
using Casper.Domain.Infrastructure.MarkdownDocuments.Commands;

namespace Casper.Domain.Infrastructure.MarkdownDocuments
{
    public abstract class MarkdownDocument : FileMetadata
    {
        protected MarkdownDocument(PublishMarkdownDocument command)
            : this(command.RelativeUri, command.Title, command.Content, command.Published, command.Author)
        {
        }

        protected MarkdownDocument(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, relativeUri + ".md", published, author)
        {
            Title = title;
            Content = content;
        }

        public string Content { get; }
        public string Title { get; }
    }
}