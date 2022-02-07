﻿using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Mobile.UnitTests.Mocks;
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

        public static IEnumerable<object[]> SelectSupplierViewModelData =>
          new List<object[]>
          {
                new object[] { null, null, null },
                new object[] { new MockWarehouseCompany(), null, null },
                new object[] { null, new MockNavigationService(), null }
          };

        [Theory, MemberData(nameof(SelectSupplierViewModelData))]
        public void ArgumentNullException(ICompany company, INavigationService navigationService, IPageDialogService dialog)
        {
            Assert.Throws<ArgumentNullException>(
                () => new SelectSupplierViewModel(company, navigationService, dialog)
            );
        }

        [Fact]
        public void Suppliers()
        {
            Assert.NotEmpty(_app.CurrentViewModel<SelectSupplierViewModel>().Suppliers);
        }

        [Fact]
        public void ChangeSelectedDateCommand()
        {
            var vm = _app.CurrentViewModel<SelectSupplierViewModel>();
            vm.CurrentDate = DateTime.Now.AddDays(1);
            vm.ChangeSelectedDateCommand.Execute();
            Assert.NotEmpty(vm.Suppliers);
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
