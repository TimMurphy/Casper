using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure.MarkdownDocuments.Events;

namespace Casper.Domain.Features.BlogPosts.Events
{
    public class PublishedBlogPost : PublishedMarkdownDocument
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public PublishedBlogPost(PublishBlogPost command)
            : base(command)
        {
        }

        public PublishedBlogPost(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}