using System;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;
using Xunit;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class PutAwayViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .GoToPutAway();

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
        public async Task ScannerDisabled()
        {
            await _app.GoBackAsync();
            Assert.Equal(
                ScannerState.Opened,
                _app.Scanner.State
            );
        }

        [Fact]
        public void AlertMessageIfScannerCanNotBeEnabled()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile.Application(
                new MockPlatformInitializer(
                    scanner: new FailedBarcodeScanner(new InvalidOperationException("Error message")),
                    pageDialogService: dialog
                )
            ).GoToPutAway();
            Assert.Contains(
                new DialogPage {
                    Title = "Scanner initialization error",
                    Message = "Error message",
                    CancelButton = "Ok"
                },
                dialog.ShownDialogs
            );
        }

        [Fact]
        public async Task AlertMessageIfScannerCanNotBeDisabled()
        {
            var dialog = new MockPageDialogService();
            var app = WarehouseMobile.Application(
                new MockPlatformInitializer(
                    scanner: new FailedBarcodeScanner(new InvalidOperationException("Error message text")),
                    pageDialogService: dialog
                )
            ).GoToPutAway();
            await app.GoBackAsync();
            Assert.Contains(
                new DialogPage
                {
                    Title = "Scanner deinitialization error",
                    Message = "Error message text",
                    CancelButton = "Ok"
                },
                dialog.ShownDialogs
            );
        }

        [Fact]
        public void ScanGoodBarcode()
        {
            Assert.Equal(
                new MockWarehouseGood("1", 1, "123456"),
                WarehouseMobile.Application(
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
                ).GoToPutAway()
                 .Scan("123456")
                 .CurrentViewModel<PutAwayViewModel>()
                 .WarehouseGood
            );
        }

        [Fact]
        public void CheckinStorage()
        {
            Assert.NotNull(
                WarehouseMobile.Application(
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
                ).GoToPutAway()
                 .Scan("123456")
                 .CurrentViewModel<PutAwayViewModel>()
                 .CheckInStorage
            );
        }

        [Fact]
        public void CheckinStorageQuantity()
        {
            Assert.Equal(
                0,
                WarehouseMobile.Application(
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
                ).GoToPutAway()
                 .Scan("123456")
                 .CurrentViewModel<PutAwayViewModel>()
                 .CheckInQuantity
            );
        }
    }
}
