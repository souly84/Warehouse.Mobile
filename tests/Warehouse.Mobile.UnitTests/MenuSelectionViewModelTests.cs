using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class MenuSelectionViewModelTests
    {
        private App _app;

        public MenuSelectionViewModelTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
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
