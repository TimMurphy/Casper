using System;

namespace Casper.Domain.Features.Authors
{
    public class Author
    {
        public Author(string name, string email, TimeZoneInfo timeZoneInfo)
        {
            Name = name;
            Email = email;
            TimeZoneInfo = timeZoneInfo;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public TimeZoneInfo TimeZoneInfo { get; private set; }
    }
}