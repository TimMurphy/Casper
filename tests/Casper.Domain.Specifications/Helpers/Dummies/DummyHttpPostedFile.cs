using System.IO;
using System.Web;

namespace Casper.Domain.Specifications.Helpers.Dummies
{
    public class DummyHttpPostedFile : HttpPostedFileBase
    {
        private readonly string _fileContents;

        public DummyHttpPostedFile() : this("dummy http posted file")
        {
        }

        public DummyHttpPostedFile(string fileContents)
        {
            _fileContents = fileContents;
        }
        
        public override void SaveAs(string filename)
        {
            File.WriteAllText(filename, _fileContents);
        }
    }
}