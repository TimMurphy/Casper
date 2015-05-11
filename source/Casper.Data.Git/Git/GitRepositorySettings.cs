using System.IO;
using OpenMagic;

namespace Casper.Data.Git.Git
{
    public class GitRepositorySettings : IGitRepositorySettings
    {
        public GitRepositorySettings(DirectoryInfo workingDirectory, string userName, string password)
        {
            Argument.DirectoryMustExist(workingDirectory, "workingDirectory");

            WorkingDirectory = workingDirectory;
            UserName = userName;
            Password = password;
        }

        public string Password { get; }
        public string UserName { get; }
        public DirectoryInfo WorkingDirectory { get; }
    }
}