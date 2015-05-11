using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Files;

namespace Casper.Data.Git.Repositories
{
    public class FileRepository : FileRepositoryBase<UploadedFile>, IFileRepository
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public FileRepository(IFileRepositorySettings settings, IGitRepository gitRepository) 
            : base(gitRepository, settings.PublishedDirectory)
        {
        }

        public Task PublishAsync(UploadedFile uploadedFile)
        {
            return PublishAsync(uploadedFile, $"Published file '{uploadedFile.RelativePath}'.");
        }

        protected override Task WriteFileAsync(UploadedFile file)
        {
            return Task.Run(() => WriteFile(file));
        }

        private void WriteFile(UploadedFile file)
        {
            // todo: implement async
            var localFile = new FileInfo(Path.Combine(GitRepository.WorkingDirectory.FullName, file.RelativePath));

            if (localFile.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            localFile.Directory.Create();

            file.PostedFile.SaveAs(localFile.FullName);
        }
    }
}