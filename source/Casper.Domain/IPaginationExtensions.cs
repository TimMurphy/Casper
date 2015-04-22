using Casper.Domain.Infrastructure;

namespace Casper.Domain
{
    // ReSharper disable once InconsistentNaming
    public static class IPaginationExtensions
    {
        public static int SkipCountForLinqQueries(this IPagination pagination)
        {
            var skipCount = (pagination.PageNumber - 1)*pagination.PageSize;

            return skipCount;
        }
    }
}