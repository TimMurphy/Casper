namespace Casper.Data.Git.Repositories
{
    public interface IBlogPostRepositorySettings : IMarkdownDocumentRepositorySettings
    {
        string BlogDirectoryName { get; }
    }
}