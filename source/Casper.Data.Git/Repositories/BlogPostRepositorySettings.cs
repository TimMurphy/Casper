namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepositorySettings : MarkdownDocumentRepositorySettings, IBlogPostRepositorySettings
    {
        public BlogPostRepositorySettings(string publishedDirectory)
            : base(publishedDirectory)
        {
        }
    }
}