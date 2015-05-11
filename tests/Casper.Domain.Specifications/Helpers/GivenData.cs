using System;
using Casper.Core;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Specifications.Helpers
{
    public class GivenData
    {
        private readonly ISlugFactory _slugFactory;
        public ICommand Command;
        public string Content;
        public string Title;

        public GivenData(ISlugFactory slugFactory)
        {
            _slugFactory = slugFactory;
        }

        public Author Author { get; set; }
        public string MarkdownWithFrontMatter { get; set; }
        public DateTime Published { get; set; }
        public string RelativeUri { get; set; }

        public string GetBlogRelativeUri()
        {
            var uri = string.Format("{0}/{1}/{2}", "blog", Published.ToUniversalTime().ToFolders(), _slugFactory.CreateSlug(Title));

            return uri;
        }
    }
}