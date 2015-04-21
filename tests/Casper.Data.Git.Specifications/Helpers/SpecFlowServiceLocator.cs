using System;
using System.Collections.Generic;
using BoDi;
using Microsoft.Practices.ServiceLocation;

namespace Casper.Data.Git.Specifications.Helpers
{
    public class SpecFlowServiceLocator : ServiceLocatorImplBase
    {
        private readonly IObjectContainer _objectContainer;

        public SpecFlowServiceLocator(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException("DoGetInstance when key has a value has not been implemented.");
            }

            try
            {
                var value = _objectContainer.Resolve(serviceType);

                return value;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("{0} has not been registered with IObjectContainer. See {1}.BeforeScenario().", serviceType, typeof (SetupSteps)), exception);
            }
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotSupportedException();
        }
    }
}