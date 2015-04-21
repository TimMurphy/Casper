using Casper.Core;
using Casper.Domain.Features.Authors;
using LibGit2Sharp;
using Microsoft.Practices.ServiceLocation;

namespace Casper.Data.Git.Git
{
    public static class GitAuthorExtensions
    {
        public static Signature ToGitSignature(this Author author)
        {
            return ToGitSignature(author, ServiceLocator.Current.GetInstance<IClock>());
        }

        private static Signature ToGitSignature(Author author, IClock clock)
        {
            var when = author.TimeZoneInfo.ToLocalTime(clock);

            return new Signature(author.Name, author.Email, when);
        }
    }
}