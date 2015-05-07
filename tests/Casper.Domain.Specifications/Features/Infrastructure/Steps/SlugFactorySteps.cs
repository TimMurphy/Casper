using Casper.Core;
using Casper.Domain.Specifications.Helpers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Features.Infrastructure.Steps
{
    [Binding]
    public class SlugFactorySteps
    {
        private readonly GivenData _given;
        private readonly ActualData _actual;

        public SlugFactorySteps(GivenData given, ActualData actual)
        {
            _given = given;
            _actual = actual;
        }

        [When(@"I call SlugFactory\.CreateSlug\(title\)")]
        public void WhenICallSlug_CreateTitle()
        {
            _actual.Result = new SlugFactory().CreateSlug(_given.Title);
        }

        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(string expectedSlug)
        {
            _actual.Result.Should().Be(expectedSlug);
        }
    }
}