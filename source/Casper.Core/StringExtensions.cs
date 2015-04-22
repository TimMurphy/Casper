namespace Casper.Core
{
    public static class StringExtensions
    {
        public static string ToDosSlashes(this string value)
        {
            return value.Replace("/", "\\");
        }

        public static string ToUnixSlashes(this string value)
        {
            return value.Replace("\\", "/");
        }
    }
}
