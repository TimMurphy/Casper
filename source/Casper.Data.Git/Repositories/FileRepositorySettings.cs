using System.IO;

namespace Casper.Data.Git.Repositories
{
    public class FileRepositorySettings : MarkdownDocumentRepositorySettings, IFileRepositorySettings
    {
        public FileRepositorySettings(DirectoryInfo publishedDirectory)
            : base(publishedDirectory)
        {
        }
    }
}