using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Features.Pages.Events;
using Casper.Domain.Infrastructure.MarkdownDocuments;

namespace Casper.Domain.Features.Pages
{
    public class PageCommandHandler : MarkdownDocumentCommandHandler<Page, PublishPage, PublishedPage>
    {
        public PageCommandHandler(IPageRepository pageRepository)
            : base(pageRepository, publishPage => new Page(publishPage) , publishPage => new PublishedPage(publishPage)  )
        {

        }
    }
}