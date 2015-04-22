namespace Casper.Data.Git.Repositories
{
    public class PageRepositorySettings : MarkdownDocumentRepositorySettings, IPageRepositorySettings
    {
        public PageRepositorySettings(string publishedDirectory)
            : base(publishedDirectory)
        {
        }
    }
}