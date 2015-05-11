using System;
using System.Web;
using Casper.Domain.Features.Authors;

namespace Casper.Domain.Features.Files
{
    public class UploadedFile : FileMetadata
    {
        public UploadedFile(HttpPostedFileBase postedFile, string relativeUri, string relativePath, DateTimeOffset published, Author author)
            : base(relativeUri, relativePath, published, author)
        {
            PostedFile = postedFile;
        }

        public HttpPostedFileBase PostedFile { get; }
    }
}