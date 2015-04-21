using System.IO;
using System.Threading.Tasks;
using Casper.Domain.Features.Authors;

namespace Casper.Data.Git.Git
{
    public interface IGitRepository
    {
        DirectoryInfo WorkingDirectory { get; }

        Task CommitAsync(GitBranches branch, string relativePath, string comment, Author author);
        Task PushAsync(GitBranches branch);
    }
}