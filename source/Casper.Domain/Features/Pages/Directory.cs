using System.IO;

namespace Casper.Domain.Features.Pages
{
    public class Directory
    {
        public Directory(string relativeUri)
        {
            RelativeUri = relativeUri;
        }

        public string RelativeUri { get; private set; }

        public string Name
        {
            get { return Path.GetDirectoryName(RelativeUri); }
        }
    }
}