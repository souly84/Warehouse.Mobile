using Xamarin.Forms;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class XamarinFormsTests
    {
        public static App InitPrismApplication()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            var app = new App(new MockPlatformInitializer());
            Application.Current = null;
            Application.Current = app;
            return app;
        }
    }
}
