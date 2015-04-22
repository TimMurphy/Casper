using Casper.Domain.Features.Authors;

namespace Casper.Data.Git.Infrastructure.Metadata
{
    public class AuthorMetdata
    {
        public AuthorMetdata()
        {
            // YamlDotNet requires empty constructor
        }

        public AuthorMetdata(Author author)
        {
            Name = author.Name;
            Email = author.Email;
            TimeZoneId = author.TimeZoneInfo.Id;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string TimeZoneId { get; set; }
    }
}