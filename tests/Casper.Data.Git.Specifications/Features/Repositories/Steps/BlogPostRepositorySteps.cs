using System;
using System.IO;
using System.Linq;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Data.Git.Specifications.Helpers;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Repositories.Steps
{
    [Binding]
    public class BlogPostRepositorySteps
    {
        private const string BlogDirectoryName = "blog";
        private readonly ActualData _actual;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly GivenData _given;
        private readonly InvocationRecorder _invocationRecorder;
        private string _expectedBlogPostFullPath;

        public BlogPostRepositorySteps(GivenData given, ActualData actual, ScenarioHelpers scenarioHelpers, InvocationRecorder invocationRecorder)
        {
            _given = given;
            _actual = actual;
            _blogPostRepository = scenarioHelpers.BlogPostRepository;
            _invocationRecorder = invocationRecorder;
        }

        [Given(@"I have published a blog post")]
        public void GivenIHavePublishedABlogPost()
        {
            _given.Command = Dummy.PublishBlogPostCommand();
            _blogPostRepository.PublishAsync(_given.PublishBlogPostCommand).Wait();
        }

        [Given(@"Title is (.*)")]
        public void GivenTitleIs(string title)
        {
            _given.Title = title;
        }

        [Given(@"Content is (.*)")]
        public void GivenContentIs(string content)
        {
            _given.Content = content;
        }

        [Given(@"Author is (.*)")]
        public void GivenAuthorIs(string author)
        {
            var authorParts = author.Split(',');

            _given.Author = new Author(authorParts[0], authorParts[1], new Clock(TimeSpan.FromHours(10)));
        }

        [Given(@"Published is (.*)")]
        public void GivenPublishedIs(string published)
        {
            _given.Published = DateTime.Parse(published);
        }

        [When(@"I call PublishAsync\(PublishBlogPost command\)")]
        public void WhenICallPublishAsyncPublishBlogPostCommand()
        {
            _given.Command = new PublishBlogPost(_given.Title, _given.Content, _given.Published, _given.Author, BlogDirectoryName, new SlugFactory());
            _blogPostRepository.PublishAsync(_given.PublishBlogPostCommand).Wait();
        }

        [When(@"I call IsPublishedAsync\(path\)")]
        public void WhenICallIsPublishedAsyncPath()
        {
            _actual.Result = _blogPostRepository.IsPublishedAsync(_given.PublishBlogPostCommand.Path).Result;
        }

        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            _actual.Result.Should().Be(true);
        }

        [Then(@"the blog post should saved to (.*) file")]
        public void ThenTheBlogPostFileShouldBeCreated(string blogPostPath)
        {
            _expectedBlogPostFullPath = Path.Combine(_blogPostRepository.GitDirectory().FullName, blogPostPath);

            File.Exists(_expectedBlogPostFullPath).Should().BeTrue();
        }

        [Then(@"the file contents should be (.*)")]
        public void ThenTheFileContentsShouldBe(string fileContents)
        {
            fileContents = fileContents.Replace("{newline}", Environment.NewLine);

            File.ReadAllText(_expectedBlogPostFullPath).Should().Be(fileContents);
        }

        [Then(@"the file should be committed to the master branch of the git repository")]
        public void ThenTheFileShouldBeCommittedToTheMasterBranchOfTheGitRepository()
        {
            const GitBranches expectedBranch = GitBranches.Master;
            var expectedRelativePath = _given.PublishBlogPostCommand.Path;
            var expectedMessage = string.Format("Published blog post '{0}'.", _given.PublishBlogPostCommand.Title);
            var expectedAuthor = _given.Author;

            _invocationRecorder
                .CallsTo<GitRepository>(g => g.CommitAsync(expectedBranch, expectedRelativePath, expectedMessage, expectedAuthor))
                .Count().Should().Be(1);
        }

        [Then(@"the master branch should be pushed to the remote server")]
        public void ThenTheMasterBranchShouldBePushedToTheRemoteServer()
        {
            _invocationRecorder
                .CallsTo<GitRepository>(g => g.PushAsync(GitBranches.Master))
                .Count().Should().Be(1);
        }
    }
}