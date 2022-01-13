using Warehouse.Mobile.UnitTests.Extensions;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class PutAwayViewModelTests
    {
        private App _app = XamarinFormsTests.InitPrismApplication();

        public PutAwayViewModelTests()
        {
            // Go to put away
            _app.CurrentViewModel<MenuSelectionViewModel>().GoToPutAwayCommand.Execute(null);
        }

        [Fact]
        public void ReserveLocations()
        {
            Assert.NotEmpty(_app.CurrentViewModel<PutAwayViewModel>().ReserveLocations);
        }

        [Fact]
        public void RaceLocations()
        {
            Assert.NotEmpty(_app.CurrentViewModel<PutAwayViewModel>().RaceLocations);
        }

        [Fact]
        public void ScannerEnabled()
        {
            Assert.Equal(
                Core.Plugins.ScannerState.Enabled,
                _app.Scanner.State
            );
        }
    }
}
