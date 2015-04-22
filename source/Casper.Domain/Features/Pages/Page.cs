using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.Pages
{
    public class Page : MarkdownDocument
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public Page(PublishPage command)
            : base(command)
        {
        }

        public Page(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}