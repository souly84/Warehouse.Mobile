using Prism;
using Prism.Ioc;
using Prism.Services;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class NoCompanyMockPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IScanner>(new MockScanner());
            containerRegistry.RegisterInstance<IPageDialogService>(new MockPageDialogService());
        }
    }
}
