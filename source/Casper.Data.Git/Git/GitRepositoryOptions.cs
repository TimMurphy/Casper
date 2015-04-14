using System.IO;
using OpenMagic;

namespace Casper.Data.Git.Git
{
    public class GitRepositoryOptions : IGitRepositoryOptions
    {
        public GitRepositoryOptions(DirectoryInfo workingDirectory, string userName, string password)
        {
            Argument.DirectoryMustExist(workingDirectory, "workingDirectory");

            WorkingDirectory = workingDirectory;
            UserName = userName;
            Password = password;
        }

        public DirectoryInfo WorkingDirectory { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}