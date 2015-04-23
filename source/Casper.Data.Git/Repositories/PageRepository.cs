using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.Pages;
using EmptyStringGuard;
using NullGuard;
using Directory = Casper.Domain.Features.Pages.Directory;

namespace Casper.Data.Git.Repositories
{
    public class PageRepository : MarkdownDocumentRepository<Page>, IPageRepository
    {
        private string _blogDirectory;

        // ReSharper disable once SuggestBaseTypeForParameter
        public PageRepository(IPageRepositorySettings settings, IBlogPostRepositorySettings blogPostRepositorySettings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, yamlMarkdown)
        {
            _blogDirectory = Path.Combine(blogPostRepositorySettings.PublishedDirectory.FullName, blogPostRepositorySettings.BlogDirectoryName);
        }

        public Task<IEnumerable<Directory>> FindPublishedDirectoriesAsync([AllowNull, AllowEmpty] string relativeDirectory)
        {
            // todo: proper async
            return Task.Run(() => FindPublishedDirectories(relativeDirectory));
        }

        private IEnumerable<Directory> FindPublishedDirectories([AllowNull, AllowEmpty] string relativeDirectory)
        {
            var directory = Path.Combine(PublishedDirectory.FullName, relativeDirectory);

            return System.IO.Directory.EnumerateDirectories(directory)
                .Where(d => d != _blogDirectory)
                .Select(d => new Directory(d.Substring(PublishedDirectory.FullName.Length + 1).ToUnixSlashes()));
        }

        public Task<IEnumerable<Page>> FindPublishedPagesAsync([AllowNull, AllowEmpty] string relativeDirectory)
        {
            // todo: proper async
            return Task.Run(() => FindPublishedPages(relativeDirectory));
        }

        private IEnumerable<Page> FindPublishedPages([AllowNull, AllowEmpty] string relativeDirectory)
        {
            var directory = Path.Combine(PublishedDirectory.FullName, relativeDirectory);

            return from file in System.IO.Directory.EnumerateFiles(directory, "*.md")
                   let page = PageSerialization.TryDeserializeFromFile(file, PublishedDirectory, YamlMarkdown)
                   where page != null
                   select page;
        }
    }
}