using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using EbSoft.Warehouse.SDK;
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
        public void ItemCount()
        {
            Assert.Equal(
                "8/8", // by default Electrolux supplier is used
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ItemCount
            );
        }

        [Fact]
        public void SupplierName()
        {
            Assert.Equal(
                "Electrolux",
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .SupplierName
            );
        }

        [Fact]
        public async Task GoToHistory()
        {
            await _app
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .GoToHistoryCommand.ExecuteAsync();
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/SelectSupplierView/ReceptionDetailsView/HistoryView",
               _app.GetNavigationUriPath()
            );

            Assert.IsType<HistoryViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public async Task GoBack_ShowsWarningDialog()
        {
            var dialog = new MockPageDialogService();
            await WarehouseMobile.Application(
                new MockPlatformInitializer(
                    new MockWarehouseCompany(
                       new MockReceptionGood("1", 1, "1111")
                    ),
                    pageDialogService: dialog
                )
            ).GoToReceptionDetails()
             .CurrentViewModel<ReceptionDetailsViewModel>()
             .BackCommand.ExecuteAsync();
            Assert.Contains(
                new DialogPage
                {
                    Title = "Warning",
                    Message = "Are you sure you want to leave this reception?",
                    AcceptButton = "Yes",
                    CancelButton = "No"
                },
                dialog.ShownDialogs
            );
        }

        [Fact]
        public async Task GoBackToSupplier_WhenWarningDialogAccepted()
        {
            var app = WarehouseMobile.Application(
                new MockPlatformInitializer(
                    new MockWarehouseCompany(
                       new MockReceptionGood("1", 1, "1111")
                    ),
                    pageDialogService: new MockPageDialogService()
                )
            ).GoToReceptionDetails();
            await app
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .BackCommand.ExecuteAsync();
            Assert.IsType<SelectSupplierViewModel>(
                app.CurrentViewModel<object>()
            );
        }

        [Fact]
        public async Task DoesNotGoBackToSupplier_WhenWarningDialogDeclined()
        {
            var app = WarehouseMobile.Application(
                new MockPlatformInitializer(
                    new MockWarehouseCompany(
                       new MockReceptionGood("1", 1, "1111")
                    ),
                    pageDialogService: new MockPageDialogService(false)
                )
            ).GoToReceptionDetails();
            await app
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .BackCommand.ExecuteAsync();
            Assert.IsType<ReceptionDetailsViewModel>(
                app.CurrentViewModel<object>()
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
                    .Sum(reception => reception.Sum(good => good.ConfirmedQuantity))
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
                    .Sum(reception => reception.Count)
            );
        }

        [Fact]
        public async Task ReceptionValidationSendsConfirmedGoodsToServer()
        {
            var reception = new MockReception(
                "Reception01",
                new MockReceptionGood("1", 1, "1111"),
                new MockReceptionGood("2", 2, "2222"),
                new MockReceptionGood("3", 4, "3333")
            );
            var app = WarehouseMobile.Application(reception)
                .GoToReceptionDetails()
                .Scan("UknownBarcode").ClosePopup()
                .Scan("1111", "1111").ClosePopup()
                .Scan("2222");
            var commandTask = app
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .ValidateReceptionCommand
                .ExecuteAsync();
            app.ClosePopup();
            await commandTask;
            Assert.Equal(
                new List<IGoodConfirmation>
                {
                    (await new ExtraConfirmedReceptionGood(
                        new MockReceptionGood("1", 1, "1111")
                    ).PartiallyConfirmed(2)).Confirmation,
                    (await new MockReceptionGood("", 1000, "UknownBarcode", isUnknown: true).PartiallyConfirmed(1)).Confirmation,
                    (await new MockReceptionGood("1", 1, "1").FullyConfirmed()).Confirmation,
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
                    .Sum(reception => reception.Count)
            );
        }

        [Fact]
        public Task InitializeAsyncThrowsInvalidOperationExceptionWhenNoSupplierHasBeenPassed()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(
                () => new ReceptionDetailsViewModel(
                    new MockScanner(),
                    new MockPageDialogService(),
                    new MockNavigationService(),
                    new Commands(),
                    new KeyValueStorage()
                ).InitializeAsync(new NavigationParameters())
            );
        }

        [Fact]
        public async Task PopupMessageIfValidateReceptionCommandError()
        {
            var app = WarehouseMobile.Application(
               new MockPlatformInitializer(
                  new ValidateExceptionReception(
                      new InvalidOperationException("Test error message")
                  )
               )
            ).GoToReceptionDetails();
            var commnadTask = app
             .CurrentViewModel<ReceptionDetailsViewModel>()
             .ValidateReceptionCommand
             .ExecuteAsync();

            app.ClosePopup();
            await commnadTask;
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error!",
                    Message = "Synchronization failed. Test error message",
                    CancelButton = "GOT IT!"
                },
                WarehouseMobile.Popup().ShownPopups
            );
        }

        [Fact]
        public void PopupMessageIfScannedNotEAN13()
        {
            _app.Scan(new ScanningResult("1111", "code128", DateTime.Now.TimeOfDay));
            _app.ClosePopup();
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error!",
                    Message = "This barcode type is not supported",
                    CancelButton = "GOT IT!"
                },
                WarehouseMobile.Popup().ShownPopups
            );
        }

        [Fact]
        public void ScannerBeepFailureIfScannedNotEAN13()
        {
            _app.Scan(new ScanningResult("1111", "code128", DateTime.Now.TimeOfDay));
            _app.ClosePopup();
            Assert.Equal(
                1,
                (_app.Scanner as MockScanner).BeepFailureCount
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
                    .Sum(reception => reception.Count)
            );
        }

        /*
         * We scan 2222 barcode 3 times. The first scan should confirm the original good.
         * 2 extra scans should create Extra Confirmed good in the list and increase its confirmed
         * quantity to 3
         */
        [Fact]
        public void ScanExtraConfirmedItem_IncreasesConfirmedQuantity()
        {
            Assert.Equal(
                3,
                WarehouseMobile.Application(
                    new MockReceptionGood("1", 5, "1111"),
                    new MockReceptionGood("2", 1, "2222"),
                    new MockReceptionGood("4", 4, "3333")
                ).GoToReceptionDetails()
                 .Scan("2222", "2222").ClosePopup()
                 .Scan("2222").ClosePopup()
                 .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .Sum(reception => reception.Sum(good => good.ConfirmedQuantity))
            );
        }

        /*
         * Only 2 "5449000131805", "5410013108009" element should be presented in the list
         * All other elements were confirmed and should be skipped.
         * "4005176891021" this element is confirmed based on the state that is stored 
         * in key value storage.
         */
        [Fact]
        public async Task RestoreReceptionState()
        {
            var reception = new EbSoftReception(_data.JsonAsWebRequest(), 9)
                .WithConfirmationProgress(
                    @"{
                         ""5449000131805"": 1,
                         ""5410013108009"": 1,
                         ""4005176891021"": 1
                     }"
                );
            var viewModels = await reception
                .NotConfirmedOnly()
                .ToViewModelListAsync(new Commands());

            Assert.Equal(
                2,
                viewModels.Sum(vm => vm.ConfirmedQuantity)
            );
        }

        private string _data = @"[
            {""id"":""40"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328130E"",""qt"":""1"",""ean"":[""4005176635410""],""qtin"":""1"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""41"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328130E"",""qt"":""1"",""ean"":[""4005176635410""],""qtin"":""1"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""42"",""oa_dossier"":""OA831375"",""article"":""GROHE 3328120E"",""qt"":""1"",""ean"":[""4005176868498""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""43"",""oa_dossier"":""OA859840"",""article"":""GROHE 31566SD0"",""qt"":""1"",""ean"":[""4005176473234""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""44"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""45"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""46"",""oa_dossier"":""OA859840"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""1""},
            {""id"":""47"",""oa_dossier"":""OA861069"",""article"":""GROHE 31129DC1"",""qt"":""1"",""ean"":[""4005176891021""],""qtin"":""0"",""error_code"":null,""commentaire"":null,""itemType"":""electro"",""qtscanned"":""0""}]";
    }
}
