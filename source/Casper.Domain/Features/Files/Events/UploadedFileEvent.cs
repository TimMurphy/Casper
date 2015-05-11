using System.Web;
using Casper.Domain.Features.Files.Commands;
using Casper.Domain.Infrastructure.Messaging;
using EmptyStringGuard;

namespace Casper.Domain.Features.Files.Events
{
    public class UploadedFileEvent : IEvent
    {
        public UploadedFileEvent(UploadFile command)
        {
            File = command.UploadedFile;
            RelativeDirectory = command.RelativeDirectory;
            UrlFriendlyFileNameWithExtension = command.UrlFriendlyFileNameWithExtension;
        }

        [AllowEmpty]
        public string RelativeDirectory { get; private set; }
        public HttpPostedFileBase File { get; private set; }
        public string UrlFriendlyFileNameWithExtension { get; private set; }
    }
}