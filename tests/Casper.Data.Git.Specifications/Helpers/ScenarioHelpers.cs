using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Anotar.LibLog;
using BoDi;
using Casper.Data.Git.Git;
using Casper.Data.Git.Repositories;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.Pages;
using Casper.Domain.Specifications.Helpers;
using Castle.DynamicProxy;
using LibGit2Sharp;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Helpers
{
    [Binding]
    public class ScenarioHelpers
    {
        private readonly Lazy<IBlogPostRepository> _blogPostRepository;
        private readonly Lazy<IPageRepository> _pageRepository;
        private readonly Lazy<IGitRepository> _gitRepository;
        public readonly List<Action> CleanUpActions = new List<Action>();
        public DirectoryInfo GitCloneDirectory;
        public DirectoryInfo GitRemoteDirectory;
        private readonly Lazy<DirectoryInfo> _repositoryDirectory;

        public ScenarioHelpers(IObjectContainer objectContainer)
        {
            _gitRepository = new Lazy<IGitRepository>(() => GitRepositoryFactory(objectContainer.Resolve<InvocationRecorder>()));
            _blogPostRepository = new Lazy<IBlogPostRepository>(BlogPostRepositoryFactory);
            _pageRepository = new Lazy<IPageRepository>(PageRepositoryFactory);
            _repositoryDirectory = new Lazy<DirectoryInfo>(CreateSelfDeletingDirectory);
        }

        public IGitRepository GitRepository
        {
            get { return _gitRepository.Value; }
        }

        public IBlogPostRepository BlogPostRepository
        {
            get { return _blogPostRepository.Value; }
        }

        public IPageRepository PageRepository
        {
            get { return _pageRepository.Value; }
        }

        public DirectoryInfo BlogPostRepositoryWorkingDirectory { get; private set; }

        /// <summary>
        ///     Creates a temporary directory that will be deleted after scenario has completed.
        /// </summary>
        /// <returns>
        ///     The temporary directory that was created.
        /// </returns>
        public DirectoryInfo CreateSelfDeletingDirectory()
        {
            var directory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            CleanUpActions.Add(() => DeleteSelfDeletingDirectory(directory));

            return directory;
        }

        private static void DeleteSelfDeletingDirectory(DirectoryInfo directory)
        {
            LogTo.Trace("DeleteSelfDeletingDirectory(directory: {0})", directory.FullName);

            const int maximumAttempts = 3;

            for (var attempt = 0; attempt < maximumAttempts; attempt++)
            {
                try
                {
                    directory.ForceDeleteIfExists();
                    break;
                }
                catch (Exception exception)
                {
                    if (attempt == maximumAttempts - 1)
                    {
                        LogTo.ErrorException(string.Format("Could not delete directory '{0}'. FINAL ATTEMPT.", directory.FullName), exception);
                    }
                    else
                    {
                        LogTo.WarnException(string.Format("Could not delete directory '{0}'. Attempt '{1}'.", directory.FullName, attempt + 1), exception);
                    }
                }
            }
        }

        /// <summary>
        ///     Runs the clean up actions in <see cref="CleanUpActions" />.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            foreach (var cleanUpAction in CleanUpActions)
            {
                cleanUpAction();
            }
        }

        private IBlogPostRepository BlogPostRepositoryFactory()
        {
            return new BlogPostRepository(new BlogPostRepositorySettings(_repositoryDirectory.Value, "blog"), GitRepository, Dummy.YamlMarkdown());
        }

        private IPageRepository PageRepositoryFactory()
        {
            return new PageRepository(new PageRepositorySettings(_repositoryDirectory.Value), new BlogPostRepositorySettings(_repositoryDirectory.Value, "blog"), GitRepository, Dummy.YamlMarkdown());
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private IGitRepository GitRepositoryFactory(InvocationRecorder invocationRecorder)
        {
            var workingDirectory = InitGitRepository();

            var generator = new ProxyGenerator();
            var gitRepository = generator.CreateInterfaceProxyWithTarget<IGitRepository>(new GitRepository(new GitRepositorySettings(workingDirectory, "dummy", "dummy@example.com")), invocationRecorder);

            BlogPostRepositoryWorkingDirectory = workingDirectory;

            return gitRepository;
        }

        private DirectoryInfo InitGitRepository()
        {
            var sw = Stopwatch.StartNew();

            GitRemoteDirectory = CreateSelfDeletingDirectory();

            Repository.Init(GitRemoteDirectory.FullName, true);

            GitCloneDirectory = CreateSelfDeletingDirectory();

            Repository.Clone(GitRemoteDirectory.FullName, GitCloneDirectory.FullName);

            using (var repository = new Repository(GitCloneDirectory.FullName))
            {
                repository.Stage("*");
                repository.Commit("Initialized master branch.", Dummy.Signature());
            }

            LogTo.Trace("InitGitRepository took {0:N0}ms.", sw.ElapsedMilliseconds);

            return GitCloneDirectory;
        }
    }
}