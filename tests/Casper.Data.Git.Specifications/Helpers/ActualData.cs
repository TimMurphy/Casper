using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.Pages;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class ActualData
    {
        public object Result;
        public BlogPost[] BlogPosts { get; set; }
        public BlogPost BlogPost { get; set; }
        public Page[] Pages { get; set; }
        public Directory[] Directories { get; set; }
    }
}