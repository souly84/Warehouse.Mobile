using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class AppTests
    {
        private App _app;

        public AppTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            _app = new App(new MockPlatformInitializer());
        }

        [Fact]
        public void ScannerInitialized()
        {
            Assert.NotNull(
               _app.Scanner
            );
        }

        [Fact]
        public void NavigationInitialized()
        {
            Assert.NotNull(
                _app.Navigation
            );
        }

        [Fact]
        public void MenuSelectionView_AsDefaultViewOnApplicationStartup()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView",
                _app.GetNavigationUriPath()
            );
        }
    }
}
