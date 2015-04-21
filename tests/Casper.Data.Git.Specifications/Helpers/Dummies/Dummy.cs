﻿using System;
using System.IO;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure;
using FakeItEasy;
using LibGit2Sharp;

namespace Casper.Data.Git.Specifications.Helpers.Dummies
{
    public class Dummy
    {
        public static PublishBlogPost PublishBlogPostCommand()
        {
            return new PublishBlogPost("dummy title", "dummy content", DateTime.Now, Author());
        }

        public static Author Author()
        {
            return new Author("dummy name", "dummy@example.com", TimeZone.CurrentTimeZone);
        }

        public static string TextFile(DirectoryInfo directory)
        {
            var fileNameWithExtension = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}.txt", fileNameWithExtension);
            var path = Path.Combine(directory.FullName, fileName);

            File.WriteAllText(path, fileNameWithExtension);

            return fileName;
        }

        public static Signature Signature()
        {
            return new Signature("dummy name", "dummy@example.com", DateTimeOffset.Now);
        }

        public static IMarkdownParser MarkdownParser()
        {
            return A.Dummy<IMarkdownParser>();
        }

        public static ISlugFactory SlugFactory()
        {
            return new SlugFactory();
        }

        public static IYamlMarkdown YamlMarkdown()
        {
            return new YamlMarkdown();
        }

        public BlogPost BlogPost()
        {
            return new BlogPost(PublishBlogPostCommand());
        }
    }
}