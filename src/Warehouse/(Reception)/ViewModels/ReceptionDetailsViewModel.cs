using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Xamarin.Forms;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : BindableBase, IInitializeAsync, INavigatedAware
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;
        private ReceptionWithUnkownGoods _reception;
        private IList<ReceptionGoodViewModel> _receptionGoods;
        private DelegateCommand validateReceptionCommand;

        public ReceptionDetailsViewModel(IScanner scanner, IPageDialogService dialog)
        {
            _scanner = scanner;
            _dialog = dialog;
        }

        public IList<ReceptionGoodViewModel> ReceptionGoods
        {
            get => _receptionGoods;
            set => SetProperty(ref _receptionGoods, value);
        }

        public DelegateCommand ValidateReceptionCommand => validateReceptionCommand ?? (validateReceptionCommand = new DelegateCommand(async () =>
        {
            try
            {
                await _reception.Confirmation().CommitAsync();
            }
            catch (Exception ex)
            {
                await _dialog.DisplayAlertAsync("Syncro error", ex.Message, "Ok");
            }
        }));

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            if (!parameters.ContainsKey("Supplier"))
            {
                throw new InvalidOperationException("No supplier selected");
            }
            var sup = parameters.GetValue<ISupplier>("Supplier");
            _reception = new ReceptionWithUnkownGoods(await sup.Receptions.FirstAsync());
            ReceptionGoods = await _reception.NeedConfirmation().ToViewModelListAsync();
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
            catch (Exception ex)
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
                    "Scanner initialization error",
                    ex.Message,
                    "Ok"
                ).FireAndForget();
            }
        }

        protected virtual void OnScan(object sender, IScanningResult barcode)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var good = await _reception.ByBarcodeAsync(barcode.BarcodeData);
                    var goodVm = ReceptionGoods.FirstOrDefault(x => x.Equals(good));
                    if (goodVm != null)
                    {
                        goodVm.IncreaseQuantityCommand.Execute();
                        if (await good.ConfirmedAsync())
                        {
                            ReceptionGoods.Remove(goodVm);
                        }
                    }
                    else
                    {
                        ReceptionGoods.Insert(0, new ReceptionGoodViewModel(good));
                    }

                }
                catch (Exception ex)
                {
                    await _dialog.DisplayAlertAsync("Error scanning", ex.Message, "ok");
                }
            });
        }
    }
}
