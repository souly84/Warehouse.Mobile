using System;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core.Plugins;
using Xamarin.Forms;

namespace Warehouse.Mobile.ViewModels
{
    public class ScannerViewModel : BindableBase, INavigatedAware
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;

        public ScannerViewModel(IScanner scanner, IPageDialogService dialog)
        {
            _scanner = scanner;
            _dialog = dialog;
        }

        async void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                _scanner.OnScan += OnScan;
                if (_scanner.State == ScannerState.Closed)
                {
                    await _scanner.OpenAsync();
                }

                await _scanner.EnableAsync(true);
            }
            catch(Exception ex)
            {
                _scanner.OnScan -= OnScan;
                _dialog.DisplayAlertAsync(
                    "Scanner initialization error",
                    ex.Message,
                    "Ok"
                ).FireAndForget();
            }
        }

        async void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {
            try
            {
                _scanner.OnScan -= OnScan;
                await _scanner.EnableAsync(false);
            }
            catch (Exception ex)
            {
                _dialog.DisplayAlertAsync(
                    "Scanner deinitialization error",
                    ex.Message,
                    "Ok"
                ).FireAndForget();
            }
        }

        protected virtual Task OnScanAsync(IScanningResult barcode)
        {
            return Task.CompletedTask;
        }

        private void OnScan(object sender, IScanningResult barcode)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await OnScanAsync(barcode);
                }
                catch (Exception ex)
                {
                    await _dialog.DisplayAlertAsync("Error scanning", ex.Message, "Ok");
                }
            });
        }
    }
}
