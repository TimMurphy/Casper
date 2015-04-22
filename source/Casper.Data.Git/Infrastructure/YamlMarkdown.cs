using System;
using System.IO;
using System.Linq;
using Casper.Data.Git.Infrastructure.Metadata;
using OpenMagic.Extensions;
using YamlDotNet.Serialization;

namespace Casper.Data.Git.Infrastructure
{
    public class YamlMarkdown : IYamlMarkdown
    {
        public string Serialize(MarkdownMetadata metadata, string markdown)
        {
            var yaml = YamlSerialize(metadata);

            return string.Format("---\r\n{0}\r\n---\r\n{1}", yaml, markdown);
        }

        public void Deserialize(string markdownWithFrontMatter, out MarkdownMetadata markdownMetadata, out string markdown)
        {
            string metaDataText;

            SplitMarkdownWithFrontMatter(markdownWithFrontMatter, out metaDataText, out markdown);

            using (var textReader = new StringReader(metaDataText))
            {
                markdownMetadata = new Deserializer().Deserialize<MarkdownMetadata>(textReader);
            }
        }

        private static void SplitMarkdownWithFrontMatter(string markdownWithFrontMatter, out string metaData, out string content)
        {
            var lines = markdownWithFrontMatter.ToLines().ToArray();

            if (lines[0] != "---")
            {
                throw new ArgumentException("Value must start with front matter. Cannot find start of front matter.", "markdownWithFrontMatter");
            }

            var endFrontMatter = 0;

            for (var i = 1; i < lines.Length; i++)
            {
                if (lines[i] != "---")
                {
                    continue;
                }
                endFrontMatter = i;
                break;
            }

            if (endFrontMatter == 0)
            {
                throw new ArgumentException("Value must start with front matter. Cannot find end of front matter.", "markdownWithFrontMatter");
            }

            metaData = string.Join(Environment.NewLine, lines.Skip(1).Take(endFrontMatter - 1));
            content = string.Join(Environment.NewLine, lines.Skip(endFrontMatter + 1));
        }

        private string YamlSerialize(object metadata)
        {
            using (var stringWriter = new StringWriter())
            {
                new Serializer().Serialize(stringWriter, metadata);

                return stringWriter.ToString();
            }
        }
    }
}