using Prism;
using Warehouse.Core;

namespace Warehouse.Mobile.UnitTests
{
    public static class WarehouseMobile
    {
        public static App Application()
        {
            return Application(new MockWarehouseCompany());
        }

        public static App Application(ICompany company)
        {
            return Application(new MockPlatformInitializer(company));
        }

        public static App Application(IPlatformInitializer platformInitializer)
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            var app = new App(platformInitializer);
            Xamarin.Forms.Application.Current = null;
            Xamarin.Forms.Application.Current = app;
            return app;
        }
    }
}
