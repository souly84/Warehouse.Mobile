using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class AppTests
    {
        public AppTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        [Fact]
        public void Startup()
        {
            Assert.NotNull(
                new App(new MockPlatformInitializer())
            );
        }

        [Fact]
        public void ScannerInitialized()
        {
            Assert.NotNull(
                new App(new MockPlatformInitializer()).Scanner
            );
        }

        [Fact]
        public void NavigationInitialized()
        {
            Assert.NotNull(
                new App(new MockPlatformInitializer()).Navigation
            );
        }
    }
}
