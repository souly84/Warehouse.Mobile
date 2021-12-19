using Prism.Navigation;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.ViewModels;
using Xunit;

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
    }
}
