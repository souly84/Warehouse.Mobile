using System;
using System.Threading.Tasks;
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
    }
}
