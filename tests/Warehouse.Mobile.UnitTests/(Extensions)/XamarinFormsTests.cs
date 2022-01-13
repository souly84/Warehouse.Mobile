using Warehouse.Core;
using Xamarin.Forms;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class XamarinFormsTests
    {
        public static App InitPrismApplication()
        {
            return InitPrismApplication(new MockWarehouseCompany());
        }

        public static App InitPrismApplication(ICompany company)
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            var app = new App(new MockPlatformInitializer(company));
            Application.Current = null;
            Application.Current = app;
            return app;
        }
    }
}
