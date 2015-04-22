using System;
using System.IO;
using System.Linq;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Data.Git.Specifications.Helpers;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.Authors;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;
using Casper.Domain.Specifications.Helpers;
using FluentAssertions;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Repositories.BlogPostRepository.Steps
{
    [Binding]
    public class PublishAsyncSteps
    {
        private readonly ActualData _actual;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly GivenData _given;
        private readonly InvocationRecorder _invocationRecorder;
        private readonly ISlugFactory _slugFactory;
        private string _expectedBlogPostFullPath;
        private readonly DirectoryInfo _blogPostRepositoryWorkingDirectory;

        public PublishAsyncSteps(GivenData given, ActualData actual, ScenarioHelpers scenarioHelpers, InvocationRecorder invocationRecorder, ISlugFactory slugFactory)
        {
            _given = given;
            _actual = actual;
            _blogPostRepository = scenarioHelpers.BlogPostRepository;
            _blogPostRepositoryWorkingDirectory = scenarioHelpers.BlogPostRepositoryWorkingDirectory;
            _invocationRecorder = invocationRecorder;
            _slugFactory = slugFactory;
        }

        [Given(@"TimeZoneId is (.*)")]
        public void GivenTimeZoneIdIs(string timeZoneId)
        {
            Dummy.SetTimeZoneId(timeZoneId);
        }
        
        [Given(@"I have published a blog post")]
        public void GivenIHavePublishedABlogPost()
        {
            _given.BlogPost = Dummy.BlogPost();
            _blogPostRepository.PublishAsync(_given.BlogPost).Wait();
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
            var name = author.TextBeforeLast("<").Trim();
            var email = author.TextAfterLast("<").TextBeforeLast(">").Trim();

            _given.Author = new Author(name, email, Dummy.TimeZoneInfo);
        }

        [Given(@"Published is (.*)")]
        public void GivenPublishedIs(string published)
        {
            _given.Published = Dummy.TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse(published).ToUniversalTime());
        }

        [When(@"I call PublishAsync\(PublishBlogPost command\)")]
        public void WhenICallPublishAsyncPublishBlogPostCommand()
        {
            _given.BlogPost = new BlogPost(_given.GetBlogUri(), _given.Title, _given.Content, _given.Published, _given.Author);
            _blogPostRepository.PublishAsync(_given.BlogPost).Wait();
        }

        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            _actual.Result.Should().Be(true);
        }

        [Then(@"the blog post should saved to (.*) file")]
        public void ThenTheBlogPostFileShouldBeCreated(string blogPostPath)
        {
            _expectedBlogPostFullPath = Path.Combine(_blogPostRepositoryWorkingDirectory.FullName, blogPostPath);

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
            var method = _invocationRecorder.CallsTo<IGitRepository>().SingleOrDefault(i => i.Method.Name == "CommitAsync");

            method.Should().NotBeNull();

            const GitBranches expectedBranch = GitBranches.Master;
            var expectedRelativePath = string.Format("blog/{0}/{1}.md", _given.Published.ToUniversalTime().DateTime.ToFolders(), _slugFactory.CreateSlug(_given.Title)).ToDosSlashes();
            var expectedMessage = string.Format("Published blog post '{0}'.", _given.BlogPost.Title);
            var expectedAuthor = _given.Author;

            // ReSharper disable once PossibleNullReferenceException
            var arguments = method.Arguments;

            arguments[0].Should().Be(expectedBranch);
            arguments[1].Should().Be(expectedRelativePath);
            arguments[2].Should().Be(expectedMessage);

            var actualAuthor = (Author) arguments[3];

            actualAuthor.Email.Should().Be(expectedAuthor.Email);
            actualAuthor.Name.Should().Be(expectedAuthor.Name);
            actualAuthor.TimeZoneInfo.Should().Be(expectedAuthor.TimeZoneInfo);
        }

        [Then(@"the master branch should be pushed to the remote server")]
        public void ThenTheMasterBranchShouldBePushedToTheRemoteServer()
        {
            _invocationRecorder
                .CallsTo<IGitRepository>(g => g.PushAsync(GitBranches.Master))
                .Count().Should().Be(1);
        }
    }
}