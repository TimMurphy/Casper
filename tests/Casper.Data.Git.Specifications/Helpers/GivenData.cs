using System;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class GivenData
    {
        private readonly ISlugFactory _slugFactory;

        public GivenData(ISlugFactory slugFactory)
        {
            _slugFactory = slugFactory;
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

        public string GetBlogUri()
        {
            var uri = string.Format("{0}/{1}/{2}", "blog", Published.ToUniversalTime().DateTime.ToFolders(), _slugFactory.CreateSlug(Title));

            return uri;
        }

        public class GitData
        {
            public GitBranches Branch { get; set; }
            public string RelativePath { get; set; }
            public string Comment { get; set; }
            public Author Author { get; set; }
        }
    }

}