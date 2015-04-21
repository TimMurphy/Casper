using System.IO;
using YamlDotNet.Serialization;

namespace Casper.Data.Git.Infrastructure
{
    public class YamlMarkdown : IYamlMarkdown
    {
        private readonly Serializer _serializer;

        public YamlMarkdown()
        {
            _serializer = new Serializer();
        }

        public string Serialize(object metadata, string markdown)
        {
            var yaml = YamlSerialize(metadata);

            return string.Format("---\r\n{0}\r\n---\r\n{1}", yaml, markdown);
        }

        private string YamlSerialize(object metadata)
        {
            using (var stringWriter = new StringWriter())
            {
                _serializer.Serialize(stringWriter, metadata);

                return stringWriter.ToString();
            }
        }
    }
}
