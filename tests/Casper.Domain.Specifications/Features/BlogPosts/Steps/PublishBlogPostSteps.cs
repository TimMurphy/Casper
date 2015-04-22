using System;
using System.Linq;
using System.Threading.Tasks;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.BlogPosts.Events;
using Casper.Domain.Infrastructure.MarkdownDocuments;
using Casper.Domain.Infrastructure.Messaging;
using Casper.Domain.Specifications.Helpers;
using Casper.Domain.Specifications.Helpers.Dummies;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Features.BlogPosts.Steps
{
    [Binding]
    public class PublishBlogPostSteps
    {
        private readonly ActualData _actual;
        private readonly ICommandBus _commandBus;
        private readonly GivenData _given;
        private readonly InvocationRecorder _invocationRecorder;

        public PublishBlogPostSteps(GivenData given, ActualData actual, ICommandBus commandBus, IEventBus eventBus, InvocationRecorder invocationRecorder)
        {
            _given = given;
            _actual = actual;
            _commandBus = commandBus;
            _invocationRecorder = invocationRecorder;

            _given.Published = DateTime.Now;

            eventBus.SubscribeTo<PublishedBlogPost>(HandleCreatedBlogPostEvent);
        }

        private Task HandleCreatedBlogPostEvent(PublishedBlogPost @event)
        {
            _actual.PublishedEvents.Add(@event);

            return Task.FromResult(0);
        }

        [Given(@"title is (.*)")]
        public void GivenTitleIs(string givenTitle)
        {
            _given.Title = givenTitle == "valid" ? "dummy title" : givenTitle;
        }

        [Given(@"content is valid")]
        public void GivenContentIsValid()
        {
            _given.Content = "dummy content";
        }

        [Given(@"author is valid")]
        public void GivenAuthorIsValid()
        {
            _given.Author = new Author("dummy name", "dummy@example.com", Dummy.TimeZoneInfo());
        }

        [When(@"I send the PublishBlogPost command")]
        public void WhenISendThePublishBlogPostCommand()
        {
            _given.Command = new PublishBlogPost(_given.GetBlogRelativeUri(), _given.Title, _given.Content, _given.Published, _given.Author);
            _commandBus.SendCommandAsync(_given.Command).Wait();
        }

        [Then(@"the blog post should be created")]
        public void ThenTheBlogPostShouldBeCreated()
        {
            var publishAsync = _invocationRecorder.CallsTo<IMarkdownDocumentRepository<BlogPost>>().Single(m => m.Method.Name == "PublishAsync");
            var expectedBlogPost = new BlogPost((PublishBlogPost) _given.Command);
            var actualBlogPost = (BlogPost) publishAsync.Arguments[0];

            actualBlogPost.ShouldBeEquivalentTo(expectedBlogPost);
        }

        [Then(@"a PublishedBlogPost event should be published")]
        public void ThenACreatedBlogPostEventShouldBePublished()
        {
            _actual.PublishedEvents.Count(e => e.GetType() == typeof(PublishedBlogPost)).Should().Be(1, "because the PublishedBlogPost event should have been published by the EventBus");
        }
    }
}