﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Anotar.NLog;
using BoDi;
using Casper.Data.Git.Git;
using Casper.Data.Git.Repositories;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Features.BlogPosts;
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
        private readonly Lazy<IGitRepository> _gitRepository;
        public readonly List<Action> CleanUpActions = new List<Action>();
        public DirectoryInfo GitCloneDirectory;
        public DirectoryInfo GitRemoteDirectory;

        public ScenarioHelpers(IObjectContainer objectContainer)
        {
            _gitRepository = new Lazy<IGitRepository>(() => GitRepositoryFactory(objectContainer.Resolve<InvocationRecorder>()));
            _blogPostRepository = new Lazy<IBlogPostRepository>(BlogPostRepositoryFactory);
        }

        public IGitRepository GitRepository
        {
            get { return _gitRepository.Value; }
        }

        public IBlogPostRepository BlogPostRepository
        {
            get { return _blogPostRepository.Value; }
        }

        /// <summary>
        ///     Creates a temporary directory that will be deleted after scenario has completed.
        /// </summary>
        /// <returns>
        ///     The temporary directory that was created.
        /// </returns>
        public DirectoryInfo CreateSelfDeletingDirectory()
        {
            var directory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            CleanUpActions.Add(() => { directory.ForceDeleteIfExists(); });

            return directory;
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
            return new BlogPostRepository(new BlogPostRepositorySettings(CreateSelfDeletingDirectory().FullName, "blog"), GitRepository, Dummy.MarkdownParser(), Dummy.SlugFactory(), Dummy.YamlMarkdown());
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private IGitRepository GitRepositoryFactory(InvocationRecorder invocationRecorder)
        {
            var workingDirectory = InitGitRepository();

            var generator = new ProxyGenerator();
            var gitRepository = generator.CreateInterfaceProxyWithTarget<IGitRepository>(new GitRepository(new GitRepositorySettings(workingDirectory, "dummy", "dummy@example.com")), invocationRecorder);

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