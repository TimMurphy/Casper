using System;
using System.IO;
using System.Linq;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.Pages;
using Casper.Domain.Infrastructure;
using LibGit2Sharp;
using OpenMagic;

namespace Casper.Data.Git.Specifications.Helpers.Dummies
{
    public class Dummy
    {
        static Dummy()
        {
            var timeZoneInfos = System.TimeZoneInfo.GetSystemTimeZones().ToArray();
            var index = RandomNumber.NextInt(0, timeZoneInfos.Length - 1);

            SetTimeZoneId(timeZoneInfos[index].Id);
        }

        public static PublishBlogPost PublishBlogPostCommand()
        {
            var published = Published();
            var dateFolder = published.ToUniversalTime().DateTime.ToFolders();
            var title = Title();
            var uri = string.Format("{0}/{1}/{2}", "blog", dateFolder, title);

            return new PublishBlogPost(uri, title, "dummy content", published, Author());
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
            return Author().ToGitSignature();
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

        public static BlogPost BlogPost(string relativeUri)
        {
            return new BlogPost(relativeUri, Title(), Content(), Published(), Author());
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

        public static string RelativeUri()
        {
            return "relative-uri-" + Guid.NewGuid();
        }

        public static string RelativeUri(string blogDirectoryName, DateTime published, string title)
        {
            return string.Format("{0}/{1}/{2}", blogDirectoryName, published.ToFolders(), SlugFactory().CreateSlug(title));
        }

        public static string Title()
        {
            return String("Title");
        }

        public static string String(string prefix)
        {
            return prefix + " " + Guid.NewGuid();
        }

        public static string Content()
        {
            return String("Content");
        }

        public static DateTimeOffset Published()
        {
            return DateTimeOffset();
        }

        public static DateTimeOffset DateTimeOffset()
        {
            return new DateTimeOffset(RandomNumber.NextInt(2000, 2014), RandomNumber.NextInt(1, 12), RandomNumber.NextInt(1, 28), RandomNumber.NextInt(0, 23), RandomNumber.NextInt(0, 59), RandomNumber.NextInt(0, 59), TimeSpan.FromHours(RandomNumber.NextInt(-13, 13)));
        }
        public static Author Author()
        {
            return new Author(String("Name"), Email(), TimeZoneInfo);
        }

        private static string Email()
        {
            return Guid.NewGuid() + "@example.com";
        }

        public static string Title(string titleFormat, DateTime published)
        {
            return titleFormat.Replace("{day}", published.Day.ToString());
        }

        public static Page Page()
        {
            return Page(RelativeUri());
        }

        public static Page Page(string relativeUri)
        {
            return new Page(relativeUri, Title(), Content(), Published(), Author());
        }
    }
}
