using System;
using System.Web;
using Casper.Domain.Features.Authors;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Features.Files.Commands
{
    public class UploadFile : ICommand
    {
        public UploadFile(HttpPostedFileBase uploadedFile, string relativeDirectory, string urlFriendlyFileNameWithExtension, DateTimeOffset published, Author author)
        {
            UploadedFile = uploadedFile;
            RelativeDirectory = relativeDirectory;
            UrlFriendlyFileNameWithExtension = urlFriendlyFileNameWithExtension;
            Published = published;
            Author = author;
        }

        public Author Author { get; private set; }
        public DateTimeOffset Published { get; private set; }
        public string RelativeDirectory { get; private set; }
        public HttpPostedFileBase UploadedFile { get; private set; }
        public string UrlFriendlyFileNameWithExtension { get; private set; }
    }
}