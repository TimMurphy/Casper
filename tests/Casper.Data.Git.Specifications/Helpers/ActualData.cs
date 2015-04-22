using Casper.Domain.Features.BlogPosts;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class ActualData
    {
        public object Result;
        public BlogPost[] BlogPosts { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}