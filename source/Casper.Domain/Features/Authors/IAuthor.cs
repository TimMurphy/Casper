using Casper.Core;

namespace Casper.Domain.Features.Authors
{
    public interface IAuthor
    {
        string Name { get; }
        string Email { get; }
        IClock Clock { get; }
    }
}