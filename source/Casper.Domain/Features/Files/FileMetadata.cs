using System;
using System.IO;
using Casper.Domain.Features.Authors;

namespace Casper.Domain.Features.Files
{
    public abstract class FileMetadata
    {
        protected FileMetadata(string relativeUri, string relativePath, DateTimeOffset published, Author author)
        {
            RelativeUri = relativeUri;
            RelativePath = relativePath;
            Published = published;
            Author = author;
        }

        public Author Author { get; }
        public string Name => Path.GetFileNameWithoutExtension(RelativeUri);
        public DateTimeOffset Published { get; }
        public string RelativePath { get; }
        public string RelativeUri { get; }
    }
}