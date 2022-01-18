using System;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class MenuSelectionViewModelTests
    {
        private App _app = WarehouseMobile.Application();

        [Fact]
        public void ArgumentNullReferenceExceptionWhenNavigationServiceNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MenuSelectionViewModel(null));
        }

        [Fact]
        public void SuppliersNavigation()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/SelectSupplierView",
                 _app.GoToSuppliers().GetNavigationUriPath()
            );

            Assert.IsType<SelectSupplierViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public void PutAwayNavigation()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/PutAwayView",
                _app.GoToPutAway().GetNavigationUriPath()
            );

            Assert.IsType<PutAwayViewModel>(_app.CurrentViewModel<object>());
        }
    }
}
