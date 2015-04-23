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
        public object Command;
        private string _directory;

        public GivenData(ISlugFactory slugFactory)
        {
            _slugFactory = slugFactory;
            Git = new GitData();
        }

        public PublishBlogPost PublishBlogPostCommand
        {
            get { return (PublishBlogPost) Command; }
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Published { get; set; }
        public GitData Git { get; set; }
        public Author Author { get; set; }
        public BlogPost BlogPost { get; set; }
        public DateTime PublishedFirstDate { get; set; }
        public DateTime PublishedLastDate { get; set; }
        public string TitleFormat { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public IPagination Pagination
        {
            get { return new Pagination(PageNumber, PageSize); }
        }

        public string MarkdownWithFrontMatter { get; set; }

        public string Directory
        {
            get { return _directory; }
            set { _directory = value == "empty string" ? "" : value; }
        }

        public string RelativeUri { get; set; }

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