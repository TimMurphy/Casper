using System;
using System.IO;
using Anotar.LibLog;
using BoDi;
using Casper.Data.Git.Git;
using Casper.Data.Git.Repositories;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using Casper.Domain.Specifications.Helpers.Dummies;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace Casper.Domain.Specifications.Steps
{
    [Binding]
    public class SetupSteps
    {
        private readonly IObjectContainer _objectContainer;
        private DirectoryInfo _publishedDirectory;

        public SetupSteps(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _publishedDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));

            var eventBus = new EventBus();
            var commandBus = new CommandBus(eventBus);
            var gitRepository = Dummy.GitRepository();
            var blogPostRepository = new BlogPostRepository(gitRepository, _publishedDirectory, Dummy.MarkdownParser());

            Configuration.Configure(commandBus, blogPostRepository);

            // Register Instances
            _objectContainer.RegisterInstanceAs(commandBus, typeof(ICommandBus));
            _objectContainer.RegisterInstanceAs(eventBus, typeof(IEventBus));
            _objectContainer.RegisterInstanceAs(gitRepository, typeof(IGitRepository));
            _objectContainer.RegisterInstanceAs(blogPostRepository, typeof(IBlogPostRepository));

            // Register Types
            _objectContainer.RegisterTypeAs<SlugFactory, ISlugFactory>();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            TryDeleteDirectory(_publishedDirectory);
            TryDeleteDirectory(_objectContainer.Resolve<IGitRepository>().WorkingDirectory);
        }

        private void TryDeleteDirectory(DirectoryInfo directory)
        {
            try
            {
                directory.ForceDelete();
            }
            catch (Exception exception)
            {
                LogTo.WarnException(string.Format("Could not delete '{0}' directory.", directory.FullName), exception);
            }
        }
    }
}
