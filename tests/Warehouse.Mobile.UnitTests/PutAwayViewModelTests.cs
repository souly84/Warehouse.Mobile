using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Extensions;
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

        public static IEnumerable<object[]> PutAwayViewModelData =>
          new List<object[]>
          {
                new object[] { null, null, null, null, null },
                new object[] { new MockScanner(), null, null, null, null },
                new object[] { null, new MockPageDialogService(), null, null, null },
                new object[] { null, null, new MockWarehouseCompany(), null, null },
                new object[] { null, null, null, null, new MockNavigationService() },
                new object[] { new MockScanner(), new MockPageDialogService(), null, null, null },
                new object[] { new MockScanner(), new MockPageDialogService(), new MockWarehouseCompany(), null, null }
          };

        [Theory, MemberData(nameof(PutAwayViewModelData))]
        public void ArgumentNullException(
            IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            ICommands commands,
            INavigationService navigationService)
        {
            Assert.Throws<ArgumentNullException>(
                () => new PutAwayViewModel(
                    scanner,
                    dialog,
                    company,
                    commands,
                    navigationService
                )
            );
        }

        [Fact]
        public void DestinationBarcodeScanning()
        {
            Assert.Equal(
                "1111",
                WarehouseMobile.Application(
                    new MockWarehouseGood("1", 5, "1111")
                ).GoToPutAway()
                 .Scan(new ScanningResult("1111", "code128", DateTime.Now.TimeOfDay))
                 .CurrentViewModel<PutAwayViewModel>()
                 .DestinationBarcode
             );
        }

        [Fact]
        public void DestinationBarcodeScanning_StatusMessage()
        {
            Assert.Equal(
                "Item successfully assigned",
                WarehouseMobile.Application(
                    new MockStorage(
                        "Storage111",
                        new MockWarehouseGood("1", 5, "1111"),
                        new MockWarehouseGood("2", 5, "2222")
                    )
                ).GoToPutAway()
                .Scan("2222")
                .Scan(new ScanningResult("Storage111", "code128", DateTime.Now.TimeOfDay))
                .CurrentViewModel<PutAwayViewModel>()
                .StatusMessage
             );
        }

        [Fact]
        public void DestinationBarcodeScanning_NavigationToQuantityPopup()
        {
            var app = WarehouseMobile.Application(
                new MockStorage(
                    "Storage111",
                    new MockWarehouseGood("1", 5, "1111"),
                    new MockWarehouseGood("2", 5, "2222")
                )
            ).GoToPutAway();
            app.Scan("2222"); // Scan good
            app.CurrentViewModel<PutAwayViewModel>().CheckInQuantity = 2;
            app.Scan(new ScanningResult("Storage111", "code128", DateTime.Now.TimeOfDay));
            Assert.IsType<QuantityToMovePopupViewModel>(
                app.CurrentViewModel<object>()
            );
        }

        [Fact]
        public void ReserveLocations()
        {
            Assert.NotEmpty(
                WarehouseMobile.Application(
                    new MockWarehouseGood("1", 5, "1111")
                ).GoToPutAway()
                 .Scan("1111")
                 .CurrentViewModel<PutAwayViewModel>()
                 .ReserveLocations
             );
        }

        [Fact]
        public void RaceLocations()
        {
            Assert.NotEmpty(
                WarehouseMobile.Application(
                   new MockWarehouseGood("1", 5, "1111")
                ).GoToPutAway()
                .Scan("1111")
                .CurrentViewModel<PutAwayViewModel>()
                .RaceLocations
            );
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
        public void AlertMessage_WhenNoPutAwayStorageForGood()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile.Application(
                new MockPlatformInitializer(
                    new MockWarehouseCompany(
                        new MockStorage(
                            new WarehouseGoodWithoutPutawayStorage(
                                new MockWarehouseGood("1", 1, "1111")
                            )
                        )
                    ),
                    pageDialogService: dialog
                )
            ).GoToPutAway()
             .Scan("1111");
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error",
                    Message = "This item is not present in the check in area",
                    CancelButton = "Ok"
                },
                dialog.ShownDialogs
            );
        }

        [Fact]
        public void AlertMessage_OnScanError()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile
                .Application(dialog)
                .GoToPutAway()
                .Scan(new ErrorScanningResult("Error message22"));
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error scanning",
                    Message = "Error message22",
                    CancelButton = "Ok"
                },
                dialog.ShownDialogs
            );
        }

        [Fact]
        public void AlertMessageIfScannerCanNotBeEnabled()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile.Application(
                new MockPlatformInitializer(
                    scanner: new FailedBarcodeScanner(
                        new InvalidOperationException("Error message")
                    ),
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
            await WarehouseMobile.Application(
                new MockPlatformInitializer(
                    scanner: new FailedBarcodeScanner(
                        new InvalidOperationException("Error message text")
                    ),
                    pageDialogService: dialog
                )
            ).GoToPutAway()
             .GoBackAsync();
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
                        new MockWarehouseGood("1", 1, "123456")
                    )
                ).GoToPutAway()
                 .Scan("123456")
                 .CurrentViewModel<PutAwayViewModel>()
                 .WarehouseGood
            );
        }

        [Fact]
        public void PutAwayStorage_Location()
        {
            var good = new MockWarehouseGood("1", 1, "1111");
            Assert.Equal(
                "MockStorage1",
                WarehouseMobile.Application(
                    good.WithPutAway(new MockStorage("MockStorage1", good))
                ).GoToPutAway()
                 .Scan("1111")
                 .CurrentViewModel<PutAwayViewModel>()
                 .PutAwayStorage
                 .Location
            );
        }

        [Fact]
        public async Task PutAwayStorage_GoodsQuantity()
        {
            var good = new MockWarehouseGood("1", 4, "1111");
            Assert.Equal(
                4,
                await WarehouseMobile.Application(
                    new MockStorage(
                        "MockStorage1",
                        good.WithPutAway(new MockStorage("ST01", good))
                    )
                ).GoToPutAway()
                 .Scan("1111")
                 .CurrentViewModel<PutAwayViewModel>()
                 .PutAwayStorage.Quantity.ValueAsync()
            );
        }

        [Fact]
        public void CheckinStorageQuantity()
        {
            var good = new MockWarehouseGood("1", 3, "123456");
            Assert.Equal(
                3,
                WarehouseMobile.Application(
                    new MockWarehouseCompany(
                        good.WithPutAway(new MockStorage("ST01", good))
                    )
                ).GoToPutAway()
                 .Scan("123456")
                 .CurrentViewModel<PutAwayViewModel>()
                 .CheckInQuantity
            );
        }
    }
}
