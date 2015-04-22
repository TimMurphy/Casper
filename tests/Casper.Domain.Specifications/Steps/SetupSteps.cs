using System;
using System.IO;
using BoDi;
using Casper.Data.Git.Git;
using Casper.Data.Git.Repositories;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.Pages;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using Casper.Domain.Specifications.Helpers;
using Casper.Domain.Specifications.Helpers.Dummies;
using Castle.DynamicProxy;
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

            var invocationRecorder = new InvocationRecorder();
            var generator = new ProxyGenerator();


            var eventBus = new EventBus();
            var commandBus = new CommandBus(eventBus);
            var gitRepository = Dummy.GitRepository();
            var slugFactory = Dummy.SlugFactory();
            var yamlMatter = Dummy.YamlMatter();
            var blogPostRepository = generator.CreateInterfaceProxyWithTarget<IBlogPostRepository>(new BlogPostRepository(new BlogPostRepositorySettings(_publishedDirectory, "blog"), gitRepository, yamlMatter), invocationRecorder);
            var pageRepository = generator.CreateInterfaceProxyWithTarget<IPageRepository>(new PageRepository(new PageRepositorySettings(_publishedDirectory), gitRepository, yamlMatter), invocationRecorder);

            Configuration.Configure(commandBus, blogPostRepository, pageRepository);

            // Register Instances
            _objectContainer.RegisterInstanceAs(commandBus, typeof(ICommandBus));
            _objectContainer.RegisterInstanceAs(eventBus, typeof(IEventBus));
            _objectContainer.RegisterInstanceAs(gitRepository, typeof(IGitRepository));
            _objectContainer.RegisterInstanceAs(blogPostRepository, typeof(IBlogPostRepository));
            _objectContainer.RegisterInstanceAs(pageRepository, typeof(IPageRepository));
            _objectContainer.RegisterInstanceAs(slugFactory, typeof(ISlugFactory));
            _objectContainer.RegisterInstanceAs(invocationRecorder, typeof(InvocationRecorder));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _publishedDirectory.ForceDeleteIfExists();
            _objectContainer.Resolve<IGitRepository>().WorkingDirectory.ForceDeleteIfExists();
        }
    }
}
