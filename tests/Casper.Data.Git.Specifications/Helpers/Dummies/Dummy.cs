using System;
using System.IO;
using Casper.Domain.Features.Authors;
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
            return new PublishBlogPost("dummy title", "dummy content", DateTime.Now, Author(), "blog", new SlugFactory());
        }

        public static IAuthor Author()
        {
            return Author(DateTime.Now);
        }

        public static IAuthor Author(DateTime dateTime)
        {
            return new Author("dummy name", "dummy@example.com", new StaticClock(dateTime));
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
    }
}