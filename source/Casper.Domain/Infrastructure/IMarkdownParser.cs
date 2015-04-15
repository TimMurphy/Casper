namespace Casper.Domain.Infrastructure
{
    public interface IMarkdownParser
    {
        string ToHtml(string contents);
    }
}