using BoDi;
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
            // Register Types
            _objectContainer.RegisterTypeAs<InvocationRecorder, InvocationRecorder>();
        }
    }
}