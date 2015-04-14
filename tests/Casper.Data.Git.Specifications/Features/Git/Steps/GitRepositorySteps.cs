using System;
using System.IO;
using System.Linq;
using Casper.Data.Git.Git;
using Casper.Data.Git.Specifications.Helpers;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using FluentAssertions;
using LibGit2Sharp;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Git.Steps
{
    [Binding]
    public class GitRepositorySteps
    {
        private readonly IGitRepository _gitRepository;
        private readonly GivenData _given;
        private readonly DirectoryInfo _gitRemoteDirectory;
        private readonly DirectoryInfo _gitCloneDirectory;

        public GitRepositorySteps(GivenData given, ScenarioHelpers scenarioHelpers)
        {
            _given = given;
            _gitRepository = scenarioHelpers.GitRepository;
            _gitCloneDirectory = scenarioHelpers.GitCloneDirectory;
            _gitRemoteDirectory = scenarioHelpers.GitRemoteDirectory;
        }

        [Given(@"branch is valid")]
        public void GivenBranchIsValid()
        {
            _given.Git.Branch = GitBranches.Master;
        }

        [Given(@"relativePath is valid")]
        public void GivenRelativePathIsValid()
        {
            _given.Git.RelativePath = Dummy.TextFile(_gitRepository.WorkingDirectory);
        }

        [Given(@"comment is valid")]
        public void GivenCommentIsValid()
        {
            _given.Git.Comment = "dummy commit message";
        }

        [Given(@"author is valid")]
        public void GivenAuthorIsValid()
        {
            // Use a time other than now so Author.When can be properly tested.
            var now = new DateTime(2015, 1, 2, 3, 4, 5);

            _given.Git.Author = Dummy.Author(now);
        }

        [When(@"I call CommitAsync\(branch, relativePath, comment\)")]
        public void WhenICallCommitAsyncBranchRelativePathComment()
        {
            _gitRepository.CommitAsync(_given.Git.Branch, _given.Git.RelativePath, _given.Git.Comment, _given.Git.Author).Wait();
        }

        [Then(@"the git repository should be updated with relativePath and comment")]
        public void ThenTheGitRepositoryShouldBeUpdatedWithRelativePathAndComment()
        {
            using (var repository = new Repository(_gitCloneDirectory.FullName))
            {
                var commit = repository.Commits.First();

                commit.Author.Name.Should().Be(_given.Git.Author.Name);
                commit.Author.Email.Should().Be(_given.Git.Author.Email);
                commit.Author.When.Should().Be(_given.Git.Author.Clock.Now);
                commit.Message.Should().Be(_given.Git.Comment + '\n');

                repository.Head.Name.Should().Be(_given.Git.Branch.ToString().ToLower());
            }
        }

        [Given(@"a git repository has repositories to push to its remote origin")]
        public void GivenAGitRepositoryHasRepositoriesToPushToItsRemoteOrigin()
        {
            GivenBranchIsValid();
            GivenRelativePathIsValid();
            GivenCommentIsValid();
            GivenAuthorIsValid();
            WhenICallCommitAsyncBranchRelativePathComment();
        }

        [When(@"I call PushAsync\(branch\)")]
        public void WhenICallPushAsyncBranch()
        {
            _gitRepository.PushAsync(_given.Git.Branch).Wait();
        }

        [Then(@"the git repository should be pushed to its remote origin")]
        public void ThenTheGitRepositoryShouldBePushedToItsRemoteOrigin()
        {
            using (var repository = new Repository(_gitRemoteDirectory.FullName))
            {
                var commit = repository.Commits.First();

                commit.Author.Name.Should().Be(_given.Git.Author.Name);
                commit.Author.Email.Should().Be(_given.Git.Author.Email);
                commit.Author.When.Should().Be(_given.Git.Author.Clock.Now);
                commit.Message.Should().Be(_given.Git.Comment + '\n');

                repository.Head.Name.Should().Be(_given.Git.Branch.ToString().ToLower());
            }
        }
    }
}