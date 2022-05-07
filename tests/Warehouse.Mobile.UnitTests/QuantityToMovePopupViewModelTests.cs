using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.UnitTests.Mocks;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class QuantityToMovePopupViewModelTests
    {
        [Fact]
        public void QuantityToMovePopupViewModelOnTheTopOfTheStack()
        {
            Assert.IsType<QuantityToMovePopupViewModel>(
                WarehouseMobile
                    .Application()
                    .GoToQuantityToMovePopup()
                    .CurrentViewModel<object>()
            );
        }

        [Theory, MemberData(nameof(QuantityToMovePopupViewModelData))]
        public void ArgumentNullException(
            ICommands commands,
            INavigationService navigationService,
            ICompany company)
        {
            Assert.Throws<ArgumentNullException>(
                () => new QuantityToMovePopupViewModel(commands, navigationService, company)
            );
        }

        [Fact]
        public async Task SetQuantityCommand_PositiveValueIncreasesQuantityToMoveOn10X()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            await app
                .CurrentViewModel<QuantityToMovePopupViewModel>()
                .SetQuantityCommand
                .ExecuteAsync("3");

            Assert.Equal(
                13,
                app.CurrentViewModel<QuantityToMovePopupViewModel>().QuantityToMove
            );
        }

        [Fact]
        public async Task SetQuantityCommand_NegativeValueDecreasesQuantityToMoveOn10X()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            app.CurrentViewModel<QuantityToMovePopupViewModel>().QuantityToMove = 23;
            await app
                .CurrentViewModel<QuantityToMovePopupViewModel>()
                .SetQuantityCommand
                .ExecuteAsync("-1");

            Assert.Equal(
                2,
                app.CurrentViewModel<QuantityToMovePopupViewModel>().QuantityToMove
            );
        }

        [Fact]
        public async Task DestinationLocation()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            Assert.Equal(
                "Storage111",
                app
                    .CurrentViewModel<QuantityToMovePopupViewModel>()
                    .DestinationLocation
            );
        }

        [Fact]
        public async Task OriginLocation()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            Assert.NotNull(
                app
                    .CurrentViewModel<QuantityToMovePopupViewModel>()
                    .OriginLocation
            );
        }

        [Fact]
        public async Task CancelCommand()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            await app
                .CurrentViewModel<QuantityToMovePopupViewModel>()
                .CancelCommand.ExecuteAsync();
            Assert.IsType<PutAwayViewModel>(await app.WaitViewModel<PutAwayViewModel>());
        }

        [Fact]
        public async Task ValidateCommand_NavigatesBackToPutAwayViewModel()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            await app
                .CurrentViewModel<QuantityToMovePopupViewModel>()
                .ValidateCommand.ExecuteAsync();
            Assert.IsType<PutAwayViewModel>(await app.WaitViewModel<PutAwayViewModel>());
        }

        [Fact(Skip = "Need to implement proper assert")]
        public async Task ValidateCommand_SendMovementRequestToTheServer()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            await app
                .CurrentViewModel<QuantityToMovePopupViewModel>()
                .ValidateCommand.ExecuteAsync();
            Assert.IsType<PutAwayViewModel>(await app.WaitViewModel<PutAwayViewModel>());
        }

        private static async Task<App> AppInQuantityToMovePopupStateAsync()
        {
            var app = WarehouseMobile.Application(
                new MockWarehouse(
                     new MockStorage(
                        "Storage111",
                        new MockWarehouseGood("1", 5, "1111"),
                        new MockWarehouseGood("2", 5, "2222")
                    )
                )
            ).GoToPutAway();
            app.Scan("2222") // Scan good
               .CurrentViewModel<PutAwayViewModel>()
               .CheckInQuantity = 2;
            app.Scan(new ScanningResult("Storage111", "code128", DateTime.Now.TimeOfDay));
            // Scanning barcodes happens in async mode that's why the code should wait here
            await app.WaitViewModel<QuantityToMovePopupViewModel>();
            return app;
        }

        public static IEnumerable<object[]> QuantityToMovePopupViewModelData =>
          new List<object[]>
          {
                new object[] { null, null, null },
                new object[] { null, new MockNavigationService(), new MockWarehouseCompany(), },
                new object[] { new Commands(), null, new MockWarehouseCompany() },
                new object[] { new Commands(), new MockNavigationService(), null },
          };
    }
}
