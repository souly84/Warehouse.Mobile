using System;
using Warehouse.Mobile.UnitTests.Extensions;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class MenuSelectionViewModelTests
    {
        private App _app = XamarinFormsTests.InitPrismApplication();

        [Fact]
        public void ArgumentNullReferenceExceptionWhenNavigationServiceNull()
        {
            _app.CurrentViewModel<MenuSelectionViewModel>()
                .GoToAvailableSuppliersCommand.Execute(null);
            Assert.Throws<ArgumentNullException>(() => new MenuSelectionViewModel(null));
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

            Assert.IsType<SelectSupplierViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public void PutAwayNavigation()
        {
            _app.CurrentViewModel<MenuSelectionViewModel>()
                .GoToPutAwayCommand.Execute(null);
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/PutAwayView",
                _app.GetNavigationUriPath()
            );

            Assert.IsType<PutAwayViewModel>(_app.CurrentViewModel<object>());
        }
    }
}
