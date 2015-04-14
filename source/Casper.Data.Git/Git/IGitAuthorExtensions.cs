using Casper.Domain.Features.Authors;
using LibGit2Sharp;

namespace Casper.Data.Git.Git
{
    // ReSharper disable once InconsistentNaming
    public static class IGitAuthorExtensions
    {
        public static Signature ToSignature(this IAuthor author)
        {
            return new Signature(author.Name, author.Email, author.Clock.Now);
        }
    }
}