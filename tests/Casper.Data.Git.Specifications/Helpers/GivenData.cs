using System;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class GivenData
    {
        public GivenData(object command)
        {
            Git = new GitData();
        }

        public object Command;
        public PublishBlogPost PublishBlogPostCommand { get { return (PublishBlogPost) Command; } }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Published { get; set; }
        public GitData Git { get; set; }
        public Author Author { get; set; }
        public BlogPost BlogPost { get; set; }

        public class GitData
        {
            public GitBranches Branch { get; set; }
            public string RelativePath { get; set; }
            public string Comment { get; set; }
            public Author Author { get; set; }
        }
    }

}