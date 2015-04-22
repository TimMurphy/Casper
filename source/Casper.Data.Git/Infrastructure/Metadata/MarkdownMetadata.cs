using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Data.Git.Infrastructure.Metadata
{
    public class MarkdownMetadata
    {
        public MarkdownMetadata()
        {
            // YamlDotNet requires empty constructor
        }

        public MarkdownMetadata(MarkdownDocument markdownDocument)
        {
            Title = markdownDocument.Title;
            Published = markdownDocument.Published.ToString("o");
            Author = new AuthorMetdata(markdownDocument.Author);
        }

        public string Title { get; set; }
        public string Published { get; set; }
        public AuthorMetdata Author { get; set; }
    }
}