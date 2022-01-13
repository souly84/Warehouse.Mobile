using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.UnitTests.Extensions;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class ReceptionDetailsViewModelTests
    {
        private App _app = XamarinFormsTests.InitPrismApplication();

        public ReceptionDetailsViewModelTests()
        {
            // Go to reception details
            _app.CurrentViewModel<MenuSelectionViewModel>().GoToAvailableSuppliersCommand.Execute(null);
            _app.CurrentViewModel<SelectSupplierViewModel>()
               .Suppliers.First()
               .GoToReceptionDetailsCommand.Execute();
        }

        [Fact]
        public void ReceptionGoods()
        {
            Assert.NotEmpty(_app.CurrentViewModel<ReceptionDetailsViewModel>().ReceptionGoods);
        }

        [Fact]
        public void ScannerEnabled()
        {
            Assert.Equal(
                ScannerState.Enabled,
                _app.Scanner.State
            );
        }

        [Fact]
        public async Task ScannerDisabled()
        {
            await _app.GoBackAsync();
            Assert.Equal(
                ScannerState.Opened,
                _app.Scanner.State
            );
        }
    }
}
