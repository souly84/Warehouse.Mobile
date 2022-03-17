using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet.Commands;
using Prism.Navigation;
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
                new object[] { null, null, null, null },
                new object[] { new MockWarehouseCompany(), null, null, null, },
                new object[] { null, new MockOverlay(), new Commands(), new MockNavigationService() },
                new object[] { new MockWarehouseCompany(), null, new Commands(), new MockNavigationService() },
                new object[] { new MockWarehouseCompany(), new MockOverlay(), null, new MockNavigationService() },
                new object[] { new MockWarehouseCompany(), new MockOverlay(), new Commands(), null }
          };

        [Theory, MemberData(nameof(SelectSupplierViewModelData))]
        public void ArgumentNullException(
            ICompany company,
            IOverlay overlay,
            ICommands commands,
            INavigationService navigationService)
        {
            Assert.Throws<ArgumentNullException>(
                () => new SelectSupplierViewModel(company, overlay, commands, navigationService)
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
            var vm = WarehouseMobile
                .Application(
                    new MockWarehouseCompany(
                        new MockSupplier(
                            "Electrolux",
                            new MockReception(
                                "1",
                                DateTime.Now.AddDays(1)
                            )
                        )
                    )
                )
                .GoToSuppliers()
                .CurrentViewModel<SelectSupplierViewModel>();
            vm.SelectedDate = DateTime.Now.AddDays(1);
            vm.ChangeSelectedDateCommand.Execute();
            Assert.NotEmpty(vm.Suppliers);
        }

        [Fact]
        public void SelectedDate_TodayByDefault()
        {
            Assert.Equal(
                DateTime.Now.Date,
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
