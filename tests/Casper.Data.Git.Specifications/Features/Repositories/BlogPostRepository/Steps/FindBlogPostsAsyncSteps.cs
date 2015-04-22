using System;
using System.Linq;
using Casper.Data.Git.Specifications.Helpers;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.BlogPosts;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Repositories.BlogPostRepository.Steps
{
    [Binding]
    public class FindBlogPostsAsyncSteps
    {
        private readonly GivenData _given;
        private readonly ActualData _actual;
        private readonly IBlogPostRepository _blogPostRepository;

        public FindBlogPostsAsyncSteps(GivenData given, ActualData actual, ScenarioHelpers scenarioHelpers)
        {
            _given = given;
            _actual = actual;
            _blogPostRepository = scenarioHelpers.BlogPostRepository;
        }

        [Given(@"the first blog post was published (.*)")]
        public void GivenTheFirstBlogPostWasPublished(DateTime publishedFirstDate)
        {
            _given.PublishedFirstDate = publishedFirstDate.AddHours(0-publishedFirstDate.Hour);
        }

        [Given(@"the last blog post was published (.*)")]
        public void GivenTheLastBlogPostWasPublished(DateTime publishedLastDate)
        {
            _given.PublishedLastDate = publishedLastDate.AddHours(0 - publishedLastDate.Hour);
        }

        [Given(@"the title of each blog post is '(.*)'")]
        public void GivenTheTitleOfEachBlogPostIs(string titleFormat)
        {
            _given.TitleFormat = titleFormat;
        }

        [Given(@"one blog post is published every day")]
        public void GivenOneBlogPostIsPublishedEveryDay()
        {
            for (var published = _given.PublishedFirstDate; published <= _given.PublishedLastDate; published = published.AddDays(1))
            {
                var title = Dummy.Title(_given.TitleFormat, published);
                var relativeUri = Dummy.RelativeUri("blog", published, title);
                var blogPost = new BlogPost(relativeUri, title, Dummy.Content(), published, Dummy.Author());

                _blogPostRepository.PublishAsync(blogPost).Wait();
            }
        }

        [Given(@"page number is (.*)")]
        public void GivenPageNumberIs(int pageNumber)
        {
            _given.PageNumber = pageNumber;
        }

        [Given(@"page size is (.*)")]
        public void GivenPageSizeIs(int pageSize)
        {
            _given.PageSize = pageSize;
        }

        [When(@"I call FindPublishedBlogPostsAsync\(pagination\)")]
        public void WhenICallFindPublishedBlogPostsAsyncPagination()
        {
            _actual.BlogPosts = _blogPostRepository.FindPublishedBlogPostsAsync(_given.Pagination).Result.ToArray();
        }

        [Then(@"(.*) blog posts should be returned")]
        public void ThenBlogPostsShouldBeReturned(int expectedCount)
        {
            _actual.BlogPosts.Length.Should().Be(expectedCount);
        }

        [Then(@"the relative uri of each blog post should be")]
        public void ThenTheRelativeUriOfEachBlogPostShouldBe(Table table)
        {
            _actual.BlogPosts.Select(b => b.RelativeUri).ShouldAllBeEquivalentTo(table.Rows.Select(r => r[0]));
        }
    }
}
