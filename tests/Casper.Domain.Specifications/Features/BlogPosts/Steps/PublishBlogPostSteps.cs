using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.BlogPosts.Events;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using Casper.Domain.Specifications.Helpers;
using FakeItEasy.ExtensionSyntax.Full;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Features.BlogPosts.Steps
{
    [Binding]
    public class PublishBlogPostSteps
    {
        private readonly ActualData _actual;
        private readonly ICommandBus _commandBus;
        private readonly IGitRepository _gitRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly GivenData _given;
        private readonly ISlugFactory _slugFactory;
        private readonly string _blogDirectory;

        public PublishBlogPostSteps(GivenData given, ActualData actual, ICommandBus commandBus, IEventBus eventBus, IGitRepository gitRepository, IBlogPostRepository blogPostRepository, ISlugFactory slugFactory)
        {
            _given = given;
            _actual = actual;
            _commandBus = commandBus;
            _gitRepository = gitRepository;
            _blogPostRepository = blogPostRepository;
            _slugFactory = slugFactory;

            _blogDirectory = "blog";

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
            _given.Author = new Author("dummy name", "dummy@example.com", new Clock(TimeSpan.FromHours(10)));
        }

        [When(@"I send the PublishBlogPost command")]
        public void WhenISendThePublishBlogPostCommand()
        {
            _given.Command = new PublishBlogPost(_given.Title, _given.Content, DateTime.Now, _given.Author, _blogDirectory, _slugFactory);
            _commandBus.SendCommandAsync(_given.Command).Wait();
        }

        [Then(@"the blog post should be created")]
        public void ThenTheBlogPostShouldBeCreated()
        {
            _blogPostRepository.IsPublishedAsync(((PublishBlogPost)_given.Command).Path).Result.Should().BeTrue();
        }

        [Then(@"a PublishedBlogPost event should be published")]
        public void ThenACreatedBlogPostEventShouldBePublished()
        {
            _actual.PublishedEvents.Count(e => e.GetType() == typeof(PublishedBlogPost)).Should().Be(1, "because the PublishedBlogPost event should have been published by the EventBus");
        }
    }
}