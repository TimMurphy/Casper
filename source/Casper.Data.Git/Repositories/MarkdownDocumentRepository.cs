using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Data.Git.Repositories
{
    public abstract class MarkdownDocumentRepository<TDocument> : IMarkdownDocumentRepository<TDocument> where TDocument : MarkdownDocument
    {
        private readonly IGitRepository _gitRepository;
        private readonly DirectoryInfo _publishedDirectory;
        private readonly IYamlMarkdown _yamlMarkdown;

        protected MarkdownDocumentRepository(IMarkdownDocumentRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
        {
            _gitRepository = gitRepository;
            _publishedDirectory = new DirectoryInfo(settings.PublishedDirectory);
            _yamlMarkdown = yamlMarkdown;
        }

        public async Task PublishAsync(TDocument markdownDocument)
        {
            await WriteFileAsync(markdownDocument);
            await CommitFileAsync(markdownDocument);
            await PublishFileAsync(markdownDocument);
            await _gitRepository.PushAsync(GitBranches.Master);
        }

        private async Task PublishFileAsync(TDocument markdownDocument)
        {
            try
            {
                // todo: use true async method.
                await Task.Run(() => PublishFile(markdownDocument));
            }
            catch (Exception)
            {
                UndoCommit(markdownDocument);
                UndoWriteFile(markdownDocument);
                throw;
            }
        }

        private static void UndoCommit(TDocument markdownDocument)
        {
            throw new NotImplementedException("UndoCommit(TDocument markdownDocument)");
        }

        private async Task CommitFileAsync(TDocument markdownDocument)
        {
            try
            {
                await _gitRepository.CommitAsync(GitBranches.Master, GetRelativePath(markdownDocument), string.Format("Published blog post '{0}'.", markdownDocument.Title), markdownDocument.Author);
            }
            catch (Exception)
            {
                UndoWriteFile(markdownDocument);
                throw;
            }
        }

        private static string GetRelativePath(TDocument markdownDocument)
        {
            return (markdownDocument.RelativeUri + ".md").Replace("/", "\\");
        }

        private static void UndoWriteFile(TDocument markdownDocument)
        {
            throw new NotImplementedException("todo: UndoWriteFile(TDocument markdownDocument)");
        }

        private Task WriteFileAsync(TDocument markdownDocument)
        {
            // todo: use true async method.
            return Task.Run(() => WriteFile(markdownDocument));
        }

        private void WriteFile(TDocument markdownDocument)
        {
            var path = new FileInfo(Path.Combine(_gitRepository.WorkingDirectory.FullName, GetRelativePath(markdownDocument)));
            var contents = GetFileContents(markdownDocument);

            if (path.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(path.Directory.FullName);
            File.WriteAllText(path.FullName, contents);
        }

        private void PublishFile(TDocument markdownDocument)
        {
            var relativePath = GetRelativePath(markdownDocument);
            var gitFile = Path.Combine(_gitRepository.WorkingDirectory.FullName, relativePath);
            var publishedFile = new FileInfo(Path.Combine(_publishedDirectory.FullName, relativePath));

            if (publishedFile.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(publishedFile.Directory.FullName);
            File.Copy(gitFile, publishedFile.FullName, true);
        }

        private string GetFileContents(TDocument markdownDocument)
        {
            return markdownDocument.Serialize(_yamlMarkdown);
        }
    }
}