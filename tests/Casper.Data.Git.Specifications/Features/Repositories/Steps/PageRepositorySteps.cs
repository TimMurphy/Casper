using System.Collections.Generic;
using System.Linq;
using Casper.Data.Git.Specifications.Helpers;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Repositories.Steps
{
    [Binding]
    public class PageRepositorySteps
    {
        private readonly GivenData _given;
        private readonly ActualData _actual;
        private readonly IPageRepository _pageRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public PageRepositorySteps(GivenData given, ActualData actual, ScenarioHelpers scenarioHelpers)
        {
            _given = given;
            _actual = actual;
            _pageRepository = scenarioHelpers.PageRepository;
            _blogPostRepository = scenarioHelpers.BlogPostRepository;
        }

        [Given(@"a published page where RelativeUri is '(.*)'")]
        public void GivenAPublishedPageWhereRelativeUriIs(string relativeUri)
        {
            _pageRepository.PublishAsync(Dummy.Page(relativeUri)).Wait();
        }

        [Given(@"a published blog post where RelativeUri is '(.*)'")]
        public void GivenAPublishedBlogPostWhereRelativeUriIs(string relativeUri)
        {
            _blogPostRepository.PublishAsync(Dummy.BlogPost(relativeUri)).Wait();
        }

        [Given(@"directory is '(.*)'")]
        public void GivenDirectoryIs(string directory)
        {
            _given.Directory = directory;
        }

        [When(@"I call FindPublishedPagesAsync\(directory\)")]
        public void WhenICallFindPublishedPagesAsyncDirectory()
        {
            _actual.Pages = _pageRepository.FindPublishedPagesAsync(_given.Directory).Result.ToArray();
        }

        [When(@"I call FindPublishedDirectoriesAsync\(directory\)")]
        public void WhenICallFindPublishedDirectoriesAsyncDirectory()
        {
            _actual.Directories = _pageRepository.FindPublishedDirectoriesAsync(_given.Directory).Result.ToArray();
        }

        [Then(@"pages with the following relative uris should be returned")]
        public void ThenPagesWithTheFollowingRelativeUrisShouldBeReturned(Table table)
        {
            var actualRelativeUris = _actual.Pages.Select(p => p.RelativeUri).ToArray();
            var expectedRelativeUris = table.Rows.Select(r => r[0]).ToArray();

            actualRelativeUris.ShouldAllBeEquivalentTo(expectedRelativeUris);
        }

        [Then(@"the following directories should be returned")]
        public void ThenDirectoriesWithTheFollowingRelativeUrisShouldBeReturned(Table table)
        {
            _actual.Directories.Select(p => p.RelativeUri).ShouldAllBeEquivalentTo(table.Rows.Select(r => r[0]));
            _actual.Directories.Select(p => p.Name).ShouldAllBeEquivalentTo(table.Rows.Select(r => r[1]));
        }

        [Then(@"no directories should be returned")]
        public void ThenNoDirectoriesShouldBeReturned()
        {
            _actual.Directories.Length.Should().Be(0);
        }

        [Given(@"relativeUri is '(.*)'")]
        public void GivenRelativeUriIs(string relativeUri)
        {
            _given.RelativeUri = relativeUri;
        }

        [When(@"I call GetPublishedPageAsync\(relativeUri\)")]
        public void WhenICallGetPublishedPageAsyncRelativeUri()
        {
            _actual.Page = _pageRepository.GetPublishedPageAsync(_given.RelativeUri).Result;
        }

        [Then(@"the page should be returned")]
        public void ThenThePageShouldBeReturned()
        {
            _actual.Page.RelativeUri.Should().Be(_given.RelativeUri);
        }
    }
}
