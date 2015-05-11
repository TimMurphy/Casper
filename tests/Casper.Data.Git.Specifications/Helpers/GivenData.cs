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
        private string _directory;
        public object Command;

        public GivenData(ISlugFactory slugFactory)
        {
            _slugFactory = slugFactory;
            Git = new GitData();
        }

        public Author Author { get; set; }
        public BlogPost BlogPost { get; set; }
        public string Content { get; set; }

        public string Directory
        {
            get { return _directory; }
            set { _directory = value == "empty string" ? "" : value; }
        }

        public GitData Git { get; set; }
        public string MarkdownWithFrontMatter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IPagination Pagination => new Pagination(PageNumber, PageSize);
        public PublishBlogPost PublishBlogPostCommand => (PublishBlogPost) Command;
        public DateTimeOffset Published { get; set; }
        public DateTime PublishedFirstDate { get; set; }
        public DateTime PublishedLastDate { get; set; }
        public string RelativeUri { get; set; }
        public string Title { get; set; }
        public string TitleFormat { get; set; }

        public string GetBlogUri()
        {
            var uri = string.Format("{0}/{1}/{2}", "blog", Published.ToUniversalTime().DateTime.ToFolders(), _slugFactory.CreateSlug(Title));

            return uri;
        }

        public class GitData
        {
            public Author Author { get; set; }
            public GitBranches Branch { get; set; }
            public string Comment { get; set; }
            public string RelativePath { get; set; }
        }
    }
}