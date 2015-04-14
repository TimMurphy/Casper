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

        public SetupSteps(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var eventBus = new EventBus();
            var commandBus = new CommandBus(eventBus);
            var gitRepository = Dummy.GitRepository();
            var blogPostRepository = new BlogPostRepository(gitRepository);

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
            _objectContainer.Resolve<IGitRepository>().WorkingDirectory.ForceDelete();
        }
    }
}
