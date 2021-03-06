using System;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.BlogPosts
{
    public class BlogPost : MarkdownDocument
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public BlogPost(PublishBlogPost command)
            : base(command)
        {
        }

        public BlogPost(string relativeUri, string title, string content, DateTimeOffset published, Author author)
            : base(relativeUri, title, content, published, author)
        {
        }
    }
}