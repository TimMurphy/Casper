using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;

namespace Casper.Data.Git.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly IGitRepository _gitRepository;

        public BlogPostRepository(IGitRepository gitRepository)
        {
            _gitRepository = gitRepository;
        }

        public Task<bool> IsPublishedAsync(string path)
        {
            var fullPath = Path.Combine(_gitRepository.WorkingDirectory.FullName, path);

            return Task.FromResult(File.Exists(fullPath));
        }

        public async Task PublishAsync(PublishBlogPost command)
        {
            await WriteFileAsync(command);
            await _gitRepository.CommitAsync(GitBranches.Master, command.Path, string.Format("Published blog post '{0}'.", command.Title), PublishBlogPost.Author);
            await _gitRepository.PushAsync(GitBranches.Master);
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
    }
}