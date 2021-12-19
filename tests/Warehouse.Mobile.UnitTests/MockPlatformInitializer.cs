using Prism;
using Prism.Ioc;

namespace Warehouse.Mobile.Tests
{
    internal class MockPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Nothing to do
        }
    }
}