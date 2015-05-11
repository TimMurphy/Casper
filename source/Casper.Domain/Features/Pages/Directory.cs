using OpenMagic.Extensions;

namespace Casper.Domain.Features.Pages
{
    public class Directory
    {
        public Directory(string relativeUri)
        {
            RelativeUri = relativeUri;
        }

        public string Name => RelativeUri.TextAfterLast("/", RelativeUri);
        public string RelativeUri { get; }
    }
}