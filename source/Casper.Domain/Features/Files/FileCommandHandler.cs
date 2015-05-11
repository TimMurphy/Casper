using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Casper.Domain.Features.Files.Commands;
using Casper.Domain.Features.Files.Events;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Features.Files
{
    internal class FileCommandHandler
    {
        private readonly IFileRepository _fileRepository;

        public FileCommandHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(UploadFile command)
        {
            var relativeUri = Path.Combine(command.RelativeDirectory, command.UrlFriendlyFileNameWithExtension);
            var uploadedFile = new UploadedFile(command.UploadedFile, relativeUri, relativeUri, command.Published, command.Author);

            await _fileRepository.PublishAsync(uploadedFile);

            var @event = new UploadedFileEvent(command);

            return new[] { @event };
        }
    }
}