using System;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class QuantityToMovePopupViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .QuantityToMovePopup();

        [Fact]
        public void QuantityToMovePopupViewModelOnTheTopOfTheStack()
        {
            Assert.IsType<QuantityToMovePopupViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public void ArgumentNullException_InConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new QuantityToMovePopupViewModel(null));
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
            app.CurrentViewModel<QuantityToMovePopupViewModel>()
               .CancelCommand.Execute();
            Assert.IsType<PutAwayViewModel>(await app.WaitViewModel<PutAwayViewModel>());
        }

        [Fact]
        public async Task ValidateCommand()
        {
            var app = await AppInQuantityToMovePopupStateAsync();
            app.CurrentViewModel<QuantityToMovePopupViewModel>()
               .ValidateCommand.Execute();
            Assert.IsType<PutAwayViewModel>(await app.WaitViewModel<PutAwayViewModel>());
        }

        private async Task<App> AppInQuantityToMovePopupStateAsync()
        {
            var app = WarehouseMobile.Application(
                new MockWarehouse(
                     new ListOfEntities<IWarehouseGood>(
                        new MockWarehouseGood("1", 5, "1111"),
                        new MockWarehouseGood("2", 5, "2222").With(
                            new MockStorages(
                                new ListOfEntities<IStorage>(new MockStorage("Storage111")),
                                new ListOfEntities<IStorage>(new MockStorage("MockStorage2")),
                                new ListOfEntities<IStorage>(new MockStorage("MockStorage3"))
                            )
                        )
                     ),
                     new ListOfEntities<IStorage>(
                        new MockStorage(
                            "Storage111",
                            new MockWarehouseGood("2", 5, "2222")
                        )
                     )
                )
            ).GoToPutAway();
            app.Scan("2222") // Scan good
               .CurrentViewModel<PutAwayViewModel>().CheckInQuantity = 2;
            app.Scan(new ScanningResult("Storage111", "code128", DateTime.Now.TimeOfDay));
            // Scanning barcodes happens in async mode that's why the code should wait here
            await app.WaitViewModel<QuantityToMovePopupViewModel>();
            return app;
        }
    }
}
