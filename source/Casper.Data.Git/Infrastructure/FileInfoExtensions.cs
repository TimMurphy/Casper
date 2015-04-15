using System;
using System.IO;

namespace Casper.Data.Git.Infrastructure
{
    // todo: move to OpenMagic
    public static class FileInfoExtensions
    {
        public static FileInfo ChangeFileExtension(this FileInfo file, string newExtension)
        {
            var lengthOfFileExtension = file.Extension.Length;
            var fullNameWithoutExtension = file.FullName.Substring(0, file.FullName.Length - lengthOfFileExtension);

            return new FileInfo(fullNameWithoutExtension + newExtension);
        }
    }
}
