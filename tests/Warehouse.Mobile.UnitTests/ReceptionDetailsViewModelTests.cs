using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Extensions;
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
            WarehouseMobile.Application(dialog)
                .GoToReceptionDetails()
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
            await WarehouseMobile.Application(
                new MockPlatformInitializer(
                    scanner: new FailedBarcodeScanner(
                        new InvalidOperationException("Error message text")
                    ),
                    pageDialogService: dialog
                )
            ).GoToReceptionDetails()
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
        public void ScanBarcodeIncreasesGoodConfirmedQuantity()
        {
            Assert.Equal(
                2,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 2, "2222"),
                    new MockReceptionGood("3", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("1111", "2222")
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Sum(good => good.ConfirmedQuantity)
            );
        }

        [Fact]
        public void ScannedUnknownGoodBarcodeAppearsInReceptionGoodsCollectionAsUnknownGood()
        {
            Assert.Equal(
                4,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 2, "2222"),
                    new MockReceptionGood("3", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("UknownBarcode", "2222")
                 .ClosePopup()
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Count
            );
        }

        [Fact]
        public async Task ReceptionValidationSendsConfirmedGoodsToServer()
        {
            var reception = new MockReception(
                new MockReceptionGood("1", 1, "1111"),
                new MockReceptionGood("2", 2, "2222"),
                new MockReceptionGood("3", 4, "3333")
            );
            WarehouseMobile.Application(reception)
                .GoToReceptionDetails()
                .Scan("UknownBarcode").ClosePopup()
                .Scan("1111")
                .Scan("1111").ClosePopup()
                .Scan("2222")
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .ValidateReceptionCommand.Execute();
            Assert.Equal(
                new List<IGoodConfirmation>
                {
                    (await new ExtraConfirmedReceptionGood(
                        new MockReceptionGood("1", 1, "1111")
                    ).PartiallyConfirmed(2)).Confirmation,
                    (await new MockReceptionGood("", 1000, "UknownBarcode", isUnknown: true).PartiallyConfirmed(1)).Confirmation,
                    (await new MockReceptionGood("2", 2, "2222").PartiallyConfirmed(1)).Confirmation,
                },
                reception.ValidatedGoods
            );
        }

        [Fact]
        public void ConfirmedGoodDisappearsFromTheList()
        {
            Assert.Equal(
                2,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 2, "2222"),
                    new MockReceptionGood("3", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("2222", "2222")
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Count
            );
        }

        [Fact]
        public Task InitializeAsyncThrowsInvalidOperationExceptionWhenNoSupplierHasBeenPassed()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(
                () => new ReceptionDetailsViewModel(
                    new MockScanner(),
                    new MockPageDialogService(),
                    new MockNavigationService()
                ).InitializeAsync(new NavigationParameters())
            );
        }

        [Fact]
        public void PopupMessageIfValidateReceptionCommandError()
        {
            var dialog = new MockPageDialogService();
            WarehouseMobile.Application(
                new MockPlatformInitializer(
                   new ValidateExceptionReception(
                       new InvalidOperationException("Test error message")
                   ),
                   dialog
                )
            ).GoToReceptionDetails()
             .CurrentViewModel<ReceptionDetailsViewModel>()
             .ValidateReceptionCommand.Execute();
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error!",
                    Message = "Synchronization failed.",
                    CancelButton = "GOT IT!"
                },
                WarehouseMobile.Popup().ShownPopups.ToDialogPages()
            );
        }

        [Fact]
        public void ScanAlreadyConfirmedItem_AddsExtraConfirmedItemIntoCollection()
        {
            Assert.Equal(
                2,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 1, "2222"),
                    new MockReceptionGood("3", 1, "2222"),
                    new MockReceptionGood("4", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("2222", "2222")
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Count
            );
        }

        [Fact]
        /*
         * We scan 2222 barcode 3 times. The first scan should confirm the original good.
         * 2 extra scans should create Extra Confirmed good in the list and increase its confirmed
         * quantity to 3
         */
        public void ScanExtraConfirmedItem_IncreasesConfirmedQuantity()
        {
            Assert.Equal(
                3,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 1, "2222"),
                    new MockReceptionGood("4", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("2222")
                 .Scan("2222").ClosePopup()
                 .Scan("2222").ClosePopup()
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Sum(good => good.ConfirmedQuantity)
            );
        }        
    }
}
