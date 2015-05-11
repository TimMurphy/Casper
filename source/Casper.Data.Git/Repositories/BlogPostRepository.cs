using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : MarkdownDocumentRepository<BlogPost>, IBlogPostRepository
    {
        private readonly DirectoryInfo _blogDirectory;

        public BlogPostRepository(IBlogPostRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, yamlMarkdown)
        {
            _blogDirectory = new DirectoryInfo(Path.Combine(settings.PublishedDirectory.FullName, settings.BlogDirectoryName));
        }

        public Task<IEnumerable<BlogPost>> FindPublishedBlogPostsAsync(IPagination pagination)
        {
            LogTo.Trace("FindPublishedBlogPostsAsync(pagination: {0})", pagination);

            // todo: implement proper async
            return Task.FromResult(FindPublishedBlogPosts(pagination));
        }

        public override Task PublishAsync(BlogPost markdownDocument)
        {
            return PublishAsync(markdownDocument, $"Published blog post '{markdownDocument.Title}'.");
        }

        private IEnumerable<BlogPost> FindPublishedBlogPosts(IPagination pagination)
        {
            // The query uses two select statements so BlogPostSerialization.DeserializeFromFile is only called for requested files.
            var blogPosts = (
                from yearDirectory in Directory.EnumerateDirectories(_blogDirectory.FullName).OrderByDescending(d => d)
                from monthDirectory in Directory.EnumerateDirectories(yearDirectory).OrderByDescending(d => d)
                from dayDirectory in Directory.EnumerateDirectories(monthDirectory).OrderByDescending(d => d)
                from blogPostFile in Directory.EnumerateFiles(dayDirectory, "*.md").OrderByDescending(d => d)
                select blogPostFile)
                .Skip(pagination.SkipCountForLinqQueries())
                .Take(pagination.PageSize)
                .Select(blogPostFile => BlogPostSerialization.DeserializeFromFile(blogPostFile, PublishedDirectory, YamlMarkdown));

            return blogPosts;
        }
    }
}