using System.IO;

namespace Casper.Data.Git.Git
{
    public interface IGitRepositoryOptions
    {
        DirectoryInfo WorkingDirectory { get; }
        string UserName { get; }
        string Password { get; }
    }
}