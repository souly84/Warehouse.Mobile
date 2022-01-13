using Warehouse.Core;
using Warehouse.Core.Plugins;
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
                ScannerState.Enabled,
                _app.Scanner.State
            );
        }
        
        [Fact]
        public void ScanGoodBarcode()
        {
            var app = XamarinFormsTests.InitPrismApplication(
                new MockWarehouseCompany(
                    new MockWarehouse(
                        new ListOfEntities<IWarehouseGood>(
                            new MockWarehouseGood("1", 1, "123456")
                        ),
                        new ListOfEntities<IStorage>(
                            new MockStorage(new MockWarehouseGood("1", 1, "123456"))
                        )
                    )
                )
            );
            app.CurrentViewModel<MenuSelectionViewModel>().GoToPutAwayCommand.Execute(null);
            app.Scan("123456");
            Assert.Equal(
                new MockWarehouseGood("1", 1, "123456"),
                app.CurrentViewModel<PutAwayViewModel>().WarehouseGood
            );
        }

        [Fact]
        public void CheckinStorage()
        {
            var app = XamarinFormsTests.InitPrismApplication(
                new MockWarehouseCompany(
                    new MockWarehouse(
                        new ListOfEntities<IWarehouseGood>(
                            new MockWarehouseGood("1", 1, "123456")
                        ),
                        new ListOfEntities<IStorage>(
                            new MockStorage(
                                "ST01",
                                new MockWarehouseGood("1", 1, "123456")
                            )
                        )
                    )
                )
            );
            app.CurrentViewModel<MenuSelectionViewModel>().GoToPutAwayCommand.Execute(null);
            app.Scan("123456");
            Assert.NotNull(
                app.CurrentViewModel<PutAwayViewModel>().CheckInStorage
            );
        }

        [Fact]
        public void CheckinStorageQuantity()
        {
            var app = XamarinFormsTests.InitPrismApplication(
                new MockWarehouseCompany(
                    new MockWarehouse(
                        new ListOfEntities<IWarehouseGood>(
                            new MockWarehouseGood("1", 1, "123456")
                        ),
                        new ListOfEntities<IStorage>(
                            new MockStorage(
                                "ST01",
                                new MockWarehouseGood("1", 1, "123456")
                            )
                        )
                    )
                )
            );
            app.CurrentViewModel<MenuSelectionViewModel>().GoToPutAwayCommand.Execute(null);
            app.Scan("123456");
            Assert.Equal(
                0,
                app.CurrentViewModel<PutAwayViewModel>().CheckInQuantity
            );
        }
    }
}
