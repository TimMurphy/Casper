using System.IO;
using Casper.Data.Git.Git;
using Casper.Domain.Features.BlogPosts;
using OpenMagic.Extensions;

namespace Casper.Data.Git.Specifications.Helpers
{
    // ReSharper disable once InconsistentNaming
    internal static class IBlogPostRepositoryExtensions
    {
        internal static DirectoryInfo GitDirectory(this IBlogPostRepository repository)
        {
            return repository.GitRepository().WorkingDirectory;
        }

        internal static IGitRepository GitRepository(this IBlogPostRepository repository)
        {
            // todo: update openmagic then use GetPrivateFieldValue()
            var value = repository.GetType().GetPrivateField("_gitRepository").GetValue(repository);

            return (IGitRepository)value;
        }
    }
}