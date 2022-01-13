using Prism;
using Prism.Ioc;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;

namespace Warehouse.Mobile.UnitTests
{
    internal class MockPlatformInitializer : IPlatformInitializer
    {
        private readonly ICompany _company;

        public MockPlatformInitializer(ICompany company)
        {
            _company = company;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IScanner>(new MockScanner());
            containerRegistry.RegisterInstance<ICompany>(_company);
            containerRegistry.RegisterInstance<IPageDialogService>(new MockPageDialogService());
        }
    }
}