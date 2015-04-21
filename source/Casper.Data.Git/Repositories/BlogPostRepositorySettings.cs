namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepositorySettings : IBlogPostRepositorySettings
    {
        public BlogPostRepositorySettings(string publishedDirectory)
        {
            PublishedDirectory = publishedDirectory;
        }

        public string PublishedDirectory { get; private set; }
    }
}