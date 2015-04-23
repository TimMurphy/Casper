using System;
using System.IO;
using System.Linq;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using OpenMagic;

namespace Casper.Domain.Specifications.Helpers.Dummies
{
    public static class Dummy
    {
        public static IGitRepository GitRepository()
        {
            var value = A.Fake<IGitRepository>();
            var directory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            directory.Create();

            value.CallsTo(c => c.WorkingDirectory).Returns(directory);

            return value;
        }

        public static ISlugFactory SlugFactory()
        {
            return new SlugFactory();
        }

        public static IYamlMarkdown YamlMatter()
        {
            return A.Dummy<IYamlMarkdown>();
        }

        public static TimeZoneInfo TimeZoneInfo()
        {
            var timeZoneInfos = System.TimeZoneInfo.GetSystemTimeZones().ToArray();
            var index = RandomNumber.NextInt(0, timeZoneInfos.Length - 1);

            return timeZoneInfos[index];
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
            return new Author(String("Name"), String("Email"), TimeZoneInfo());
        }
    }
}
