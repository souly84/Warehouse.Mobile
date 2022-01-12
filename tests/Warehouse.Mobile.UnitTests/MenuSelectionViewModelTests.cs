using Warehouse.Mobile.ViewModels;
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
        }

        [Fact]
        public void SuppliersNavigation()
        {
            var viewModel = new MenuSelectionViewModel(
                _app.Navigation
            );
            viewModel.GoToAvailableSuppliersCommand.Execute(null);
            //Assert.NotNull(

            //);
        }
    }
}
