using LibGit2Sharp;

namespace Casper.Data.Git.Infrastructure
{
    public static class RepositoryExtensions
    {
        public static bool CanCommit(this Repository repository)
        {
            return repository.Index.Count > 0;
        }
    }
}
