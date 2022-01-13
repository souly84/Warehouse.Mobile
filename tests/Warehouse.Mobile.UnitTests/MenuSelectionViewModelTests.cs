using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class MenuSelectionViewModelTests
    {
        private App _app;

        public MenuSelectionViewModelTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            NavigationServiceExtensions.ResetPageNavigationRegistry();
            _app = new App(new MockPlatformInitializer());
            Application.Current = null;
            Application.Current = _app;
        }

        [Fact]
        public void SuppliersNavigation()
        {
            _app.CurrentViewModel<MenuSelectionViewModel>()
                .GoToAvailableSuppliersCommand.Execute(null);
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/SelectSupplierView?useModalNavigation=true",
                _app.GetNavigationUriPath()
            );
        }
    }
}
