using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Specifications.Helpers
{
    public class GivenData
    {
        public ICommand Command;
        public string Title;
        public string Content;
        public Author Author { get; set; }
    }
}