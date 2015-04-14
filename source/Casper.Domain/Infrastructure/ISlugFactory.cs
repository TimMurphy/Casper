namespace Casper.Domain.Infrastructure
{
    public interface ISlugFactory
    {
        string CreateSlug(string title);
    }
}