using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Files;

namespace Casper.Data.Git.Repositories
{
    public abstract class FileRepositoryBase<TFile> where TFile : FileMetadata
    {
        protected readonly IGitRepository GitRepository;
        protected readonly DirectoryInfo PublishedDirectory;

        protected FileRepositoryBase(IGitRepository gitRepository, DirectoryInfo publishedDirectory)
        {
            GitRepository = gitRepository;
            PublishedDirectory = publishedDirectory;
        }

        protected async Task PublishAsync(TFile file, string commitComment)
        {
            await WriteFileAsync(file);
            await CommitFileAsync(file, commitComment);
            await PublishFileAsync(file);
            await GitRepository.PushAsync(GitBranches.Master);
        }

        protected abstract Task WriteFileAsync(TFile file);

        private async Task CommitFileAsync(TFile file, string commitComment)
        {
            try
            {
                await GitRepository.CommitAsync(GitBranches.Master, file.RelativePath, commitComment, file.Author);
            }
            catch (Exception)
            {
                UndoWriteFile(file);
                throw;
            }
        }

        private void PublishFile(TFile file)
        {
            var relativePath = file.RelativePath;
            var gitFile = Path.Combine(GitRepository.WorkingDirectory.FullName, relativePath);
            var publishedFile = new FileInfo(Path.Combine(PublishedDirectory.FullName, relativePath));

            if (publishedFile.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(publishedFile.Directory.FullName);
            File.Copy(gitFile, publishedFile.FullName, true);
        }

        private async Task PublishFileAsync(TFile file)
        {
            try
            {
                // todo: use true async method.
                await Task.Run(() => PublishFile(file));
            }
            catch (Exception)
            {
                UndoCommit(file);
                UndoWriteFile(file);
                throw;
            }
        }

        private static void UndoCommit(TFile file)
        {
            throw new NotImplementedException(string.Format("UndoCommit(file: {0})", file.RelativeUri));
        }

        private static void UndoWriteFile(TFile file)
        {
            throw new NotImplementedException(string.Format("todo: UndoWriteFile(file: {0})", file.RelativeUri));
        }
    }
}