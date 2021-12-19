using Prism;
using Prism.Ioc;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests
{
    internal class MockPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IScanner>(new MockScanner());
            // Nothing to do
        }
    }
}