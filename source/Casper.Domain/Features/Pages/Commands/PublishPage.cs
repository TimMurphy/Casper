using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.MarkdownDocuments.Commands;

namespace Casper.Domain.Features.Pages.Commands
{
    public class PublishPage : PublishMarkdownDocument
    {
        public PublishPage(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}