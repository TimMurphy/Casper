using System.IO;

namespace Casper.Data.Git.Repositories
{
    public abstract class MarkdownDocumentRepositorySettings : IMarkdownDocumentRepositorySettings
    {
        protected MarkdownDocumentRepositorySettings(DirectoryInfo publishedDirectory)
        {
            PublishedDirectory = publishedDirectory;
        }

        public DirectoryInfo PublishedDirectory { get; private set; }
    }
}