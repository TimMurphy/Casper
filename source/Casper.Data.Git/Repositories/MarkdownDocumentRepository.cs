using System;
using System.IO;
using System.Threading.Tasks;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Data.Git.Repositories
{
    public abstract class MarkdownDocumentRepository<TDocument> : FileRepositoryBase<TDocument>, IMarkdownDocumentRepository<TDocument>
        where TDocument : MarkdownDocument
    {
        protected readonly IYamlMarkdown YamlMarkdown;

        protected MarkdownDocumentRepository(IMarkdownDocumentRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(gitRepository, settings.PublishedDirectory)
        {
            YamlMarkdown = yamlMarkdown;
        }

        public abstract Task PublishAsync(TDocument markdownDocument);

        protected override Task WriteFileAsync(TDocument markdownDocument)
        {
            // todo: use true async method.
            return Task.Run(() => WriteFile(markdownDocument));
        }

        private string GetFileContents(TDocument markdownDocument)
        {
            return markdownDocument.Serialize(YamlMarkdown);
        }

        private void WriteFile(TDocument markdownDocument)
        {
            var path = new FileInfo(Path.Combine(GitRepository.WorkingDirectory.FullName, markdownDocument.RelativePath));
            var contents = GetFileContents(markdownDocument);

            if (path.Directory == null)
            {
                throw new Exception("Expected directory would not be null.");
            }

            Directory.CreateDirectory(path.Directory.FullName);
            File.WriteAllText(path.FullName, contents);
        }
    }
}