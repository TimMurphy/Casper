using BoDi;
using Casper.Core;
using Casper.Data.Git.Specifications.Helpers.Dummies;
using Casper.Domain.Infrastructure;
using Casper.Domain.Specifications.Helpers;
using Microsoft.Practices.ServiceLocation;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Helpers
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
            _objectContainer.RegisterTypeAs<InvocationRecorder, InvocationRecorder>();
            _objectContainer.RegisterTypeAs<SlugFactory, ISlugFactory>();
            _objectContainer.RegisterInstanceAs(new DummyClock(), typeof(IClock));

            ServiceLocator.SetLocatorProvider(() => new SpecFlowServiceLocator(_objectContainer));
        }
    }
}