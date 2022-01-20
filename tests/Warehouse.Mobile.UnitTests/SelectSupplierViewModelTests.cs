﻿using System.Linq;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class SelectSupplierViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .GoToSuppliers();

        [Fact]
        public void Suppliers()
        {
            Assert.NotEmpty(_app.CurrentViewModel<SelectSupplierViewModel>().Suppliers);
        }

        [Fact]
        public void SelectedDate_TodayByDefault()
        {
            Assert.Equal(
                System.DateTime.Now.Date,
                _app.CurrentViewModel<SelectSupplierViewModel>()
                    .SelectedDate.Date
            );
        }

        [Fact]
        public void SupplierName()
        {
            Assert.Equal(
                "Electrolux",
                _app.CurrentViewModel<SelectSupplierViewModel>()
                    .Suppliers.First()
                    .Name
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
