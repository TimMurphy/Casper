using System.Collections.Generic;
using Casper.Domain.Features.BlogPosts;

namespace Casper.Domain.Specifications.Helpers
{
    public class ActualData
    {
        public readonly List<object> PublishedEvents = new List<object>();
        public object Result;
        public BlogPost BlogPost { get; set; }
    }
}
