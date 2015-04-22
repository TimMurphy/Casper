﻿using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Data.Git.Repositories
{
    public abstract class MarkdownDocumentRepository<TDocument> : IMarkdownDocumentRepository<TDocument> where TDocument : MarkdownDocument
    {
        protected readonly IGitRepository GitRepository;
        protected readonly DirectoryInfo PublishedDirectory;
        protected readonly IYamlMarkdown YamlMarkdown;

        protected MarkdownDocumentRepository(IMarkdownDocumentRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
        {
            GitRepository = gitRepository;
            PublishedDirectory = settings.PublishedDirectory;
            YamlMarkdown = yamlMarkdown;
        }

        public async Task PublishAsync(TDocument markdownDocument)
        {
            await WriteFileAsync(markdownDocument);
            await CommitFileAsync(markdownDocument);
            await PublishFileAsync(markdownDocument);
            await GitRepository.PushAsync(GitBranches.Master);
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
            throw new NotImplementedException(string.Format("UndoCommit(markdownDocument: {0})", markdownDocument.RelativeUri));
        }

        private async Task CommitFileAsync(TDocument markdownDocument)
        {
            try
            {
                await GitRepository.CommitAsync(GitBranches.Master, GetRelativePath(markdownDocument), string.Format("Published blog post '{0}'.", markdownDocument.Title), markdownDocument.Author);
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
            throw new NotImplementedException(string.Format("todo: UndoWriteFile(markdownDocument: {0})", markdownDocument.RelativeUri));
        }

        private Task WriteFileAsync(TDocument markdownDocument)
        {
            // todo: use true async method.
            return Task.Run(() => WriteFile(markdownDocument));
        }

        private void WriteFile(TDocument markdownDocument)
        {
            var path = new FileInfo(Path.Combine(GitRepository.WorkingDirectory.FullName, GetRelativePath(markdownDocument)));
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
            var gitFile = Path.Combine(GitRepository.WorkingDirectory.FullName, relativePath);
            var publishedFile = new FileInfo(Path.Combine(PublishedDirectory.FullName, relativePath));

            if (publishedFile.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(publishedFile.Directory.FullName);
            File.Copy(gitFile, publishedFile.FullName, true);
        }

        private string GetFileContents(TDocument markdownDocument)
        {
            return markdownDocument.Serialize(YamlMarkdown);
        }
    }
}