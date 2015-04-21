namespace Casper.Data.Git.Repositories
{
    public interface IBlogPostRepositorySettings
    {
        string PublishedDirectory { get; }
        string BlogDirectoryName { get; }
    }
}