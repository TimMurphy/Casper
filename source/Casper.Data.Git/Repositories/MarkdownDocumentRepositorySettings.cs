namespace Casper.Data.Git.Repositories
{
    public abstract class MarkdownDocumentRepositorySettings : IMarkdownDocumentRepositorySettings
    {
        protected MarkdownDocumentRepositorySettings(string publishedDirectory)
        {
            PublishedDirectory = publishedDirectory;
        }

        public string PublishedDirectory { get; private set; }
    }
}