using System;
using Prism.Navigation;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;
using Warehouse.Mobile.ViewModels;
using Xunit;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests
{
    public class ScannerViewModelTests
    {
        [Fact]
        public void EnablesScannerOnNavigateTo()
        {
            var scanner = new MockScanner();
            var scannerViewModel = new ScannerViewModel(
                scanner,
                new MockPageDialogService()
            ) as INavigatedAware;

            scannerViewModel.OnNavigatedTo(new NavigationParameters());

            Assert.Equal(
                ScannerState.Enabled,
                scanner.State
            );
        }

        [Fact]
        public void DisablesScannerOnNavigateFrom()
        {
            var scanner = new MockScanner();
            var scannerViewModel = new ScannerViewModel(
                scanner,
                new MockPageDialogService()
            ) as INavigatedAware;

            scannerViewModel.OnNavigatedTo(new NavigationParameters());
            scannerViewModel.OnNavigatedFrom(new NavigationParameters());

            Assert.Equal(
                ScannerState.Opened,
                scanner.State
            );
        }

        [Fact]
        public void ShowsErrorPopup_ScannerOnNavigateTo()
        {
            var dialog = new MockPageDialogService();
            (new ScannerViewModel(
                new FailedScanner(new InvalidOperationException("Test Exception")),
                dialog
            ) as INavigatedAware).OnNavigatedTo(new NavigationParameters());

            Assert.Contains(
               new DialogPage
               {
                   Title = "Scanner initialization error",
                   Message= "Test Exception",
                   CancelButton = "Ok"
               },
               dialog.ShownDialogs
            );
        }

        [Fact]
        public void ShowsErrorPopup_ScannerOnNavigateFrom()
        {
            var dialog = new MockPageDialogService();
            var scannerViewModel = new ScannerViewModel(
                new FailedScanner(new InvalidOperationException("Test Exception")),
                dialog
            ) as INavigatedAware;

            scannerViewModel.OnNavigatedFrom(new NavigationParameters());

            Assert.Contains(
              new DialogPage
              {
                  Title = "Scanner initialization error",
                  Message = "Test Exception",
                  CancelButton = "Ok"
              },
              dialog.ShownDialogs
           );
        }
    }
}
