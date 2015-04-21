using System;
using System.IO;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Infrastructure;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;

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

        public static IMarkdownParser MarkdownParser()
        {
            return A.Dummy<IMarkdownParser>();
        }

        public static ISlugFactory SlugFactory()
        {
            return new SlugFactory();
        }

        public static IYamlMarkdown YamlMatter()
        {
            return A.Dummy<IYamlMarkdown>();
        }
    }
}
