using Casper.Domain.Features.Authors;

namespace Casper.Data.Git.Infrastructure.Metadata
{
    internal class AuthorMetdata
    {
        public AuthorMetdata(Author author)
        {
            Name = author.Name;
            Email = author.Email;
            TimeZoneId = author.TimeZone.GetTimeZoneId();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string TimeZoneId { get; set; }
    }
}