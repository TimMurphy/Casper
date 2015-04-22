using System.IO;

namespace Casper.Data.Git.Repositories
{
    public class PageRepositorySettings : MarkdownDocumentRepositorySettings, IPageRepositorySettings
    {
        public PageRepositorySettings(DirectoryInfo publishedDirectory)
            : base(publishedDirectory)
        {
        }
    }
}