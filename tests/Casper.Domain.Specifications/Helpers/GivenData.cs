using Casper.Domain.Features.Authors;

namespace Casper.Domain.Specifications.Helpers
{
    public class GivenData
    {
        public object Command;
        public string Title;
        public string Content;
        public IAuthor Author { get; set; }
    }
}