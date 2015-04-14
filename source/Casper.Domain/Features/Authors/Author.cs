using Casper.Core;

namespace Casper.Domain.Features.Authors
{
    public class Author : IAuthor
    {
        public Author(string name, string email, IClock clock)
        {
            Name = name;
            Email = email;
            Clock = clock;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public IClock Clock { get; private set; }
    }
}