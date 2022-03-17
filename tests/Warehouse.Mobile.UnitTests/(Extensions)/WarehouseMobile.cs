using System.Reflection;
using Prism;
using Prism.Ioc;
using Prism.Services;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Warehouse.Core;
using Warehouse.Mobile.UnitTests.Mocks;
using Xamarin.Forms;

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
                     "Reception01",
                     receprionGoods
                 )
            );
        }

        public static App Application(params IReception[] receptions)
        {
            return Application(
                 new MockSupplier(
                     "Electrolux",
                     receptions
                 )
            );
        }

        public static App Application(params IWarehouseGood[] warehouseGoods)
        {
            return Application(
                new MockWarehouseCompany(warehouseGoods)
            );
        }

        public static App Application(params ISupplier[] suppliers)
        {
            return Application(
                new MockWarehouseCompany(suppliers)
            );
        }

        public static App Application(params IStorage[] storages)
        {
            return Application(new MockWarehouse(storages));
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
            Popup().RaiseOnInitialized();
            PopupNavigation.RestoreDefaultInstance();
            Xamarin.Forms.Application.Current = null;
            var app = new App(platformInitializer);
            Xamarin.Forms.Application.Current = app;
            Popup().ShownPopups.Clear();
            Popup().VisiblePopup.Clear();
            return app;
        }

        internal static MockPopupPlatform Popup()
        {
            return DependencyService.Resolve<IPopupPlatform>() as MockPopupPlatform;
        }
    }
}
