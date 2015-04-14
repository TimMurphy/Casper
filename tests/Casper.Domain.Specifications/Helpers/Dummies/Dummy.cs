using System;
using System.IO;
using Casper.Data.Git.Git;
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
    }
}
