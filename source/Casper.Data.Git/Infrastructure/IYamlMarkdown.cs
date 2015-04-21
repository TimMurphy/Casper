namespace Casper.Data.Git.Infrastructure
{
    public interface IYamlMarkdown
    {
        string Serialize(object metadata, string markdown);
    }
}