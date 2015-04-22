using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casper.Domain.Infrastructure.MarkdownDocuments.Commands;
using Casper.Domain.Infrastructure.MarkdownDocuments.Events;
using Casper.Domain.Infrastructure.Messaging;

namespace Casper.Domain.Infrastructure.MarkdownDocuments
{
    public abstract class MarkdownDocumentCommandHandler<TDocument, TPublishCommand, TPublishedEvent>
        where TDocument : MarkdownDocument
        where TPublishCommand : PublishMarkdownDocument
        where TPublishedEvent : PublishedMarkdownDocument
    {
        private readonly Func<TPublishCommand, TDocument> _documentFromCommandFactory;
        private readonly IMarkdownDocumentRepository<TDocument> _markdownDocumentRepository;
        private readonly Func<TPublishCommand, TPublishedEvent> _publishedEventFromCommandFactory;

        protected MarkdownDocumentCommandHandler(
            IMarkdownDocumentRepository<TDocument> markdownDocumentRepository,
            Func<TPublishCommand, TDocument> documentFromCommandFactory,
            Func<TPublishCommand, TPublishedEvent> publishedEventFromCommandFactory)
        {
            _markdownDocumentRepository = markdownDocumentRepository;
            _documentFromCommandFactory = documentFromCommandFactory;
            _publishedEventFromCommandFactory = publishedEventFromCommandFactory;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(TPublishCommand command)
        {
            var document = _documentFromCommandFactory(command);

            await _markdownDocumentRepository.PublishAsync(document);

            var @event = _publishedEventFromCommandFactory(command);

            return new[] {@event};
        }
    }
}