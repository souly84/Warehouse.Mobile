using Prism;
using Prism.Ioc;
using Prism.Services;
using Warehouse.Core;

namespace Warehouse.Mobile.UnitTests
{
    public static class WarehouseMobile
    {
        public static App Application()
        {
            return Application(new MockWarehouseCompany());
        }

        public static App Application(params IReceptionGood[] receprionGoods)
        {
            return Application(
                 new MockReception(
                     receprionGoods
                 )
            );
        }

        public static App Application(IReception reception)
        {
            return Application(
                 new MockSupplier(
                     "Electrolux",
                     reception
                 )
            );
        }

        public static App Application(params IWarehouseGood[] warehouseGoods)
        {
            return Application(
                new MockWarehouseCompany(warehouseGoods)
            );
        }

        public static App Application(ISupplier supplier)
        {
            return Application(
                new MockWarehouseCompany(supplier)
            );
        }

        public static App Application(IWarehouse warehouse)
        {
            return Application(
                new MockWarehouseCompany(warehouse)
            );
        }

        public static App Application(ICompany company)
        {
            return Application(new MockPlatformInitializer(company));
        }

        public static App Application(IPageDialogService dialogService)
        {
            return Application(
                new MockPlatformInitializer(pageDialogService: dialogService)
            );
        }

        public static App Application(IPlatformInitializer platformInitializer)
        {
            ContainerLocator.ResetContainer();
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            var app = new App(platformInitializer);
            Xamarin.Forms.Application.Current = null;
            Xamarin.Forms.Application.Current = app;
            return app;
        }
    }
}
