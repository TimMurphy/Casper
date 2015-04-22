using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Infrastructure.MarkdownDocuments.Events;

namespace Casper.Domain.Features.Pages.Events
{
    public class PublishedPage : PublishedMarkdownDocument
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public PublishedPage(PublishPage command)
            : base(command)
        {
        }

        public PublishedPage(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}