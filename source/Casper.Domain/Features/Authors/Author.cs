using System;

namespace Casper.Domain.Features.Authors
{
    public class Author
    {
        public Author(string name, string email, TimeZone timeZone)
        {
            Name = name;
            Email = email;
            TimeZone = timeZone;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public TimeZone TimeZone { get; private set; }
    }
}