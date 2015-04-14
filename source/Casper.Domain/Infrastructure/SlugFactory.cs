using System;
using System.Text;

namespace Casper.Domain.Infrastructure
{
    public class SlugFactory : ISlugFactory
    {
        public string CreateSlug(string title)
        {
            var slugBuilder = new StringBuilder();
            var slug = title
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("'", "");

            foreach (var character in slug)
            {
                if (Char.IsLetterOrDigit(character))
                {
                    slugBuilder.Append(character);
                }
                else
                {
                    slugBuilder.Append("-");
                }
            }

            slug = slugBuilder.ToString();

            // todo: refactor to OpenMagic ReplaceAll.
            while (slug.Contains("--"))
            {
                slug = slug.Replace("--", "-");
            }
            
            return slug;
        }
    }
}