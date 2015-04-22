using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.MarkdownDocuments.Commands;

namespace Casper.Domain.Features.BlogPosts.Commands
{
    public class PublishBlogPost : PublishMarkdownDocument
    {
        public PublishBlogPost(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}