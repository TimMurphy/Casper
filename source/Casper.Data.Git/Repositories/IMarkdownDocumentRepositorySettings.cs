using System.IO;

namespace Casper.Data.Git.Repositories
{
    public interface IMarkdownDocumentRepositorySettings
    {
        DirectoryInfo PublishedDirectory { get; }
    }
}