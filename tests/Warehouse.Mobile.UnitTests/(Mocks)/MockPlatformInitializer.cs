using Dotnet.Commands;
using Prism;
using Prism.Ioc;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Interfaces;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;

namespace Warehouse.Mobile.UnitTests
{
    internal class MockPlatformInitializer : IPlatformInitializer
    {
        private readonly ICompany _company;
        private readonly IScanner _scanner;
        private readonly IPageDialogService _pageDialogService;

        public MockPlatformInitializer(
            IReception reception)
            : this(
                  reception,
                  pageDialogService: null
              )
        {
        }

        public MockPlatformInitializer(
            IReception reception,
            IPageDialogService pageDialogService)
            : this(
                  company: new MockWarehouseCompany(
                      new MockSupplier(
                          "Electrolux",
                          reception
                      )
                  ),
                  pageDialogService: pageDialogService
              )
        {
        }

        public MockPlatformInitializer(
            ICompany company = null,
            IScanner scanner = null,
            IPageDialogService pageDialogService = null)
        {
            _company = company ?? new MockWarehouseCompany();
            _scanner = scanner ?? new MockScanner();
            _pageDialogService = pageDialogService ?? new MockPageDialogService();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new Commands(0).Validated());
            containerRegistry.RegisterInstance<IScanner>(_scanner);
            containerRegistry.RegisterInstance<ICompany>(_company);
            containerRegistry.RegisterInstance<IPageDialogService>(_pageDialogService);
            containerRegistry.RegisterInstance<IOverlay>(new MockOverlay());
            containerRegistry.RegisterInstance<IKeyValueStorage>(new KeyValueStorage());
            containerRegistry.RegisterInstance<IEnvironment>(new MockEnvironment());
        }
    }
}