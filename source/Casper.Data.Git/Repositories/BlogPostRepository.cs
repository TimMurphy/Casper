using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IGitRepository _gitRepository;
        private readonly DirectoryInfo _publishedDirectory;
        private readonly IMarkdownParser _markdownParser;

        public BlogPostRepository(IGitRepository gitRepository, DirectoryInfo publishedDirectory, IMarkdownParser markdownParser)
        {
            _gitRepository = gitRepository;
            _publishedDirectory = publishedDirectory;
            _markdownParser = markdownParser;
        }

        public Task<bool> IsPublishedAsync(string path)
        {
            var fullPath = Path.Combine(_gitRepository.WorkingDirectory.FullName, path);

            return Task.FromResult(File.Exists(fullPath));
        }

        public async Task PublishAsync(PublishBlogPost command)
        {
            await WriteFileAsync(command);
            await CommitFileAsync(command);
            await PublishFileAsync(command);
            await _gitRepository.PushAsync(GitBranches.Master);
        }

        private async Task PublishFileAsync(PublishBlogPost command)
        {
            try
            {
                // todo: use true async method.
                await Task.Run(() => PublishFile(command));
            }
            catch (Exception)
            {
                UndoCommit(command);
                UndoWriteFile(command);
                throw;
            }
        }

        private static void UndoCommit(PublishBlogPost command)
        {
            throw new NotImplementedException("UndoCommit(PublishBlogPost command)");
        }

        private async Task CommitFileAsync(PublishBlogPost command)
        {
            try
            {
                await _gitRepository.CommitAsync(GitBranches.Master, command.Path, string.Format("Published blog post '{0}'.", command.Title), PublishBlogPost.Author);
            }
            catch (Exception)
            {
                UndoWriteFile(command);
                throw;
            }
        }

        private static void UndoWriteFile(PublishBlogPost command)
        {
            throw new NotImplementedException("todo: UndoWriteFile(PublishBlogPost command)");
        }

        private Task WriteFileAsync(PublishBlogPost command)
        {
            // todo: use true async method.
            return Task.Run(() => WriteFile(command));
        }

        private void WriteFile(PublishBlogPost command)
        {
            var path = new FileInfo(Path.Combine(_gitRepository.WorkingDirectory.FullName, command.Path));
            var contents = command.GetFileContents();

            if (path.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(path.Directory.FullName);
            File.WriteAllText(path.FullName, contents);
        }

        private void PublishFile(PublishBlogPost command)
        {
            var path = new FileInfo(Path.Combine(_publishedDirectory.FullName, command.Path)).ChangeFileExtension(".cshtml");
            var contents = command.GetFileContents();
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