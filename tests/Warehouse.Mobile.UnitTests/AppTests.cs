using System;
using Xamarin.Forms.Xaml;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class AppTests
    {
        public AppTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
        }

        [Fact]
        public void Startup()
        {
            Assert.NotNull(
                new App(new MockPlatformInitializer())
            );
        }

        internal static string GetPathForType(Type type)
        {
            var assembly = type.Assembly;
            foreach (XamlResourceIdAttribute xria in assembly.GetCustomAttributes(typeof(XamlResourceIdAttribute), true))
            {
                if (xria.Type == type)
                    return xria.Path;
            }
            return null;
        }

        internal static string GetResourceIdForType(Type type)
        {
            var assembly = type.Assembly;
            foreach (XamlResourceIdAttribute xria in assembly.GetCustomAttributes(typeof(XamlResourceIdAttribute), true))
            {
                if (xria.Type == type)
                    return xria.ResourceId;
            }
            return null;
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
