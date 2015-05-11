using System.IO;
using OpenMagic.Extensions;

namespace Casper.Core
{
    public static class StringExtensions
    {
        public static string RemoveExtension(this string pathWithExtension)
        {
            return pathWithExtension.TextBeforeLast(".", pathWithExtension);
        }

        public static string ToDosSlashes(this string value)
        {
            return value.Replace("/", "\\");
        }

        public static string ToUnixSlashes(this string value)
        {
            return value.Replace("\\", "/");
        }

        public static string ToUrlFriendlyFileNameWithExtension(this string fileNameWithExtension)
        {
            return fileNameWithExtension.ToUrlFriendlyFileNameWithExtension(new SlugFactory());
        }

        public static string ToUrlFriendlyFileNameWithExtension(this string fileNameWithExtension, ISlugFactory slugFactory)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);
            var extension = Path.GetExtension(fileNameWithExtension);
            var urlFriendly = slugFactory.CreateSlug(fileNameWithoutExtension) + extension;

            return urlFriendly;
        }
    }
}
