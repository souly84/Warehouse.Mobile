using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;
using Warehouse.Mobile.ViewModels;
using Xunit;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class ReceptionDetailsViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .GoToReceptionDetails();

        [Fact]
        public void ReceptionGoods()
        {
            Assert.NotEmpty(
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
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
        public void AlertMessageIfOnScannError()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile.Application(
                new MockPlatformInitializer(
                    pageDialogService: dialog
                )
            ).GoToReceptionDetails()
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
            ).GoToReceptionDetails();
            Assert.Contains(
                new DialogPage
                {
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
                    scanner: new FailedBarcodeScanner(
                        new InvalidOperationException("Error message text")
                    ),
                    pageDialogService: dialog
                )
            ).GoToReceptionDetails();
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
        public void ScannedGoodBarcodeIncreaseConfirmedQuantity()
        {
            var app = WarehouseMobile
                .Application(
                    new MockWarehouseCompany(
                        new NamedMockSupplier(
                            "Electrolux",
                            new MockReception(
                                new MockReceptionGood("1", 5, "1111"),
                                new MockReceptionGood("2", 2, "2222"),
                                new MockReceptionGood("3", 4, "3333")
                            )
                        )
                    )
                ).GoToReceptionDetails()
                 .Scan("1111", "2222");
            Assert.Equal(2,
                app.CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Sum(good => good.ConfirmedQuantity)
            );
        }

        [Fact]
        public void ScannedUnknownGoodBarcodeAppearsInReceptionGoodsCollectionAsUnknownGood()
        {
            var app = WarehouseMobile
                .Application(
                    new MockWarehouseCompany(
                        new NamedMockSupplier(
                            "Electrolux",
                            new MockReception(
                                new MockReceptionGood("1", 5, "1111"),
                                new MockReceptionGood("2", 2, "2222"),
                                new MockReceptionGood("3", 4, "3333")
                            )
                        )
                    )
                ).GoToReceptionDetails()
                 .Scan("UknownBarcode", "2222");
            Assert.Equal(4,
                app.CurrentViewModel<ReceptionDetailsViewModel>()
                   .ReceptionGoods
                   .Count
            );
        }

        [Fact]
        public async Task ReceptionValidationSendsConfirmedGoodsToServer()
        {
            var reception = new MockReception(
                new MockReceptionGood("1", 5, "1111"),
                new MockReceptionGood("2", 2, "2222"),
                new MockReceptionGood("3", 4, "3333")
            );
            WarehouseMobile.Application(
                new MockWarehouseCompany(
                    new NamedMockSupplier(
                        "Electrolux",
                        reception
                    )
                )
            ).GoToReceptionDetails()
             .Scan("UknownBarcode", "1111", "2222")
             .CurrentViewModel<ReceptionDetailsViewModel>()
             .ValidateReceptionCommand.Execute();
            Assert.Equal(
                new List<IGoodConfirmation>
                {
                    (await new MockReceptionGood("1", 5, "1111").PartiallyConfirmed(1)).Confirmation,
                    (await new MockReceptionGood("2", 2, "2222").PartiallyConfirmed(1)).Confirmation,
                 //   (await new MockReceptionGood("", 1, "UknownBarcode").PartiallyConfirmed(1)).Confirmation
                },
                reception.ValidatedGoods
            );
        }
    }
}
