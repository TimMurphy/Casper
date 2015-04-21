using System;
using System.IO;
using Casper.Core;
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
        static Dummy()
        {
            SetTimeZoneId("Russian Standard Time");
        }

        public static PublishBlogPost PublishBlogPostCommand()
        {
            var published = DateTime.Now;
            var dateFolder = published.ToUniversalTime().ToFolders();
            var uri = string.Format("{0}/{1}/{2}", "blog", dateFolder, "dummy-title");

            return new PublishBlogPost(uri, "dummy title", "dummy content", published, Author());
        }

        public static Author Author()
        {
            return new Author("dummy name", "dummy@example.com", TimeZoneInfo);
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

        public static BlogPost BlogPost()
        {
            return new BlogPost(PublishBlogPostCommand());
        }

        public static TimeZoneInfo TimeZoneInfo { get; private set; }

        public static void SetTimeZoneId(string timeZoneId)
        {
            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (TimeZoneInfo.Id == TimeZoneInfo.Local.Id)
            {
                throw new Exception("Tests cannot continue because dummy TimeZoneInfo is same as local TimeZoneInfo.");
            }
        }
    }
}
