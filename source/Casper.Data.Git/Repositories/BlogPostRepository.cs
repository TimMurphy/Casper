using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IGitRepository _gitRepository;
        private readonly DirectoryInfo _publishedDirectory;
        private readonly IMarkdownParser _markdownParser;
        private readonly string _blogDirectory;
        private readonly ISlugFactory _slugFactory;
        private readonly IYamlMarkdown _yamlMarkdown;

        public BlogPostRepository(IBlogPostRepositorySettings settings, IGitRepository gitRepository, IMarkdownParser markdownParser, ISlugFactory slugFactory, IYamlMarkdown yamlMarkdown)
        {
            _gitRepository = gitRepository;
            _publishedDirectory = new DirectoryInfo(settings.PublishedDirectory);
            _markdownParser = markdownParser;
            _blogDirectory = settings.BlogDirectoryName;
            _slugFactory = slugFactory;
            _yamlMarkdown = yamlMarkdown;
        }

        public async Task PublishAsync(BlogPost blogPost)
        {
            await WriteFileAsync(blogPost);
            await CommitFileAsync(blogPost);
            await PublishFileAsync(blogPost);
            await _gitRepository.PushAsync(GitBranches.Master);
        }

        private async Task PublishFileAsync(BlogPost blogPost)
        {
            try
            {
                // todo: use true async method.
                await Task.Run(() => PublishFile(blogPost));
            }
            catch (Exception)
            {
                UndoCommit(blogPost);
                UndoWriteFile(blogPost);
                throw;
            }
        }

        private static void UndoCommit(BlogPost blogPost)
        {
            throw new NotImplementedException("UndoCommit(BlogPost blogPost)");
        }

        private async Task CommitFileAsync(BlogPost blogPost)
        {
            try
            {
                await _gitRepository.CommitAsync(GitBranches.Master, GetRelativePath(blogPost), string.Format("Published blog post '{0}'.", blogPost.Title), blogPost.Author);
            }
            catch (Exception)
            {
                UndoWriteFile(blogPost);
                throw;
            }
        }

        private string GetRelativePath(BlogPost blogPost)
        {
            var utc = blogPost.Published.ToUniversalTime();

            return string.Format("{0}/{1:D4}/{2:D2}/{3:D2}/{4}.md", _blogDirectory, utc.Year, utc.Month, utc.Day, _slugFactory.CreateSlug(blogPost.Title));
        }

        private static void UndoWriteFile(BlogPost blogPost)
        {
            throw new NotImplementedException("todo: UndoWriteFile(BlogPost blogPost)");
        }

        private Task WriteFileAsync(BlogPost blogPost)
        {
            // todo: use true async method.
            return Task.Run(() => WriteFile(blogPost));
        }

        private void WriteFile(BlogPost blogPost)
        {
            var path = new FileInfo(Path.Combine(_gitRepository.WorkingDirectory.FullName, GetRelativePath(blogPost)));
            var contents = GetFileContents(blogPost);

            if (path.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(path.Directory.FullName);
            File.WriteAllText(path.FullName, contents);
        }

        private string GetFileContents(BlogPost blogPost)
        {
            return blogPost.Serialize(_yamlMarkdown);
        }

        private void PublishFile(BlogPost blogPost)
        {
            var path = new FileInfo(Path.Combine(_publishedDirectory.FullName, GetRelativePath(blogPost))).ChangeFileExtension(".cshtml");
            var contents = GetFileContents(blogPost);
            var html = _markdownParser.ToHtml(contents);

            if (path.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(path.Directory.FullName);
            File.WriteAllText(path.FullName, html);
        }
    }
}