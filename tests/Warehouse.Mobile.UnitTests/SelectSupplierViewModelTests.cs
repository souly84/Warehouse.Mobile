using System.Linq;
using Warehouse.Mobile.UnitTests.Extensions;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class SelectSupplierViewModelTests
    {
        private App _app = XamarinFormsTests.InitPrismApplication();

        public SelectSupplierViewModelTests()
        {
            // Go to suppliers
            _app.CurrentViewModel<MenuSelectionViewModel>().GoToAvailableSuppliersCommand.Execute(null);
        }

        [Fact]
        public void Suppliers()
        {
            Assert.NotEmpty(_app.CurrentViewModel<SelectSupplierViewModel>().Suppliers);
        }

        [Fact]
        public void CurrenrDate()
        {
            Assert.NotEqual(
                System.DateTime.MinValue,
                _app.CurrentViewModel<SelectSupplierViewModel>().CurrentDate
            );
        }

        [Fact]
        public void GoToReceptionDetails()
        {
            _app.CurrentViewModel<SelectSupplierViewModel>()
                .Suppliers.First()
                .GoToReceptionDetailsCommand.Execute();
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/SelectSupplierView/ReceptionDetailsView",
               _app.GetNavigationUriPath()
            );

            Assert.IsType<ReceptionDetailsViewModel>(_app.CurrentViewModel<object>());
        }
    }
}
