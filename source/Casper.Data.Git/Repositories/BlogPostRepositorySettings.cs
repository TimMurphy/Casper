using System.IO;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepositorySettings : MarkdownDocumentRepositorySettings, IBlogPostRepositorySettings
    {
        public BlogPostRepositorySettings(DirectoryInfo publishedDirectory, string blogDirectoryName)
            : base(publishedDirectory)
        {
            BlogDirectoryName = blogDirectoryName;
        }

        public string BlogDirectoryName { get; }
    }
}