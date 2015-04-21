using System;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepositorySettings : IBlogPostRepositorySettings
    {
        public BlogPostRepositorySettings(string publishedDirectory, string blogDirectoryName)
        {
            PublishedDirectory = publishedDirectory;
            BlogDirectoryName = blogDirectoryName;
        }

        public string PublishedDirectory { get; private set; }
        public string BlogDirectoryName { get; private set; }
    }
}