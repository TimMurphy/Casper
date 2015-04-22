namespace Casper.Domain.Infrastructure
{
    public interface IPagination
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}