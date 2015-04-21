﻿using System;
using Casper.Domain.Features.Authors;

namespace Casper.Domain.Features.BlogPosts.Commands
{
    public class PublishBlogPost
    {
        public PublishBlogPost(string title, string content, DateTimeOffset published, Author author)
        {
            Title = title;
            Content = content;
            Published = published;
            Author = author;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset Published { get; private set; }
        public Author Author { get; private set; }
    }
}