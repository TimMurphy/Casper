using Casper.Domain.Features.Pages;
using Casper.Domain.Specifications.Helpers;
using Casper.Domain.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Features.Infrastructure.MarkdownDocuments.Steps
{
    [Binding]
    public class MarkdownDocumentSteps
    {
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public MarkdownDocumentSteps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"RelativeUri is (.*)")]
        public void GivenRelativeUriIs(string given)
        {
            _given.RelativeUri = given;
        }

        [When(@"I call Name")]
        public void WhenICallName()
        {
            var page = new Page(_given.RelativeUri, Dummy.Title(), Dummy.Content(), Dummy.Published(), Dummy.Author());

            _actual.Result = page.Name;
        }
    }
}
