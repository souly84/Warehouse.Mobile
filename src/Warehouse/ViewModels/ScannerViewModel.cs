using System;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.ViewModels
{
    public class ScannerViewModel : BindableBase, INavigatedAware
    {
        private readonly IScanner _scanner;
        private readonly IDialogService _dialog;

        public ScannerViewModel(IScanner scanner, IDialogService dialog)
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
                _dialog.ShowDialog(
                    "Scanner initialization error",
                    new DialogParameters($"message={ex.Message}")
                );
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
                _dialog.ShowDialog(
                    "Scanner initialization error",
                    new DialogParameters($"message={ex.Message}")
                );
            }
        }

        protected virtual void OnScan(object sender, IScanningResult barcode)
        {
        }
    }
}
