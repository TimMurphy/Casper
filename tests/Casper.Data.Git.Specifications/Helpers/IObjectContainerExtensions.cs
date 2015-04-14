using BoDi;

namespace Casper.Data.Git.Specifications.Helpers
{
    // ReSharper disable once InconsistentNaming
    internal static class IObjectContainerExtensions
    {
        internal static void RegisterInstance<TType>(this IObjectContainer objectContainer, TType instance)
        {
            objectContainer.RegisterInstanceAs(instance, typeof(TType));
        }
    }
}
