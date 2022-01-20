using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.Reception;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly IPageDialogService _dialog;
        private ReceptionWithExtraConfirmedGoods _reception;
        private ObservableCollection<ReceptionGoodViewModel> _receptionGoods;
        private DelegateCommand validateReceptionCommand;

        public ReceptionDetailsViewModel(IScanner scanner, IPageDialogService dialog)
            : base(scanner, dialog)
        {
            _dialog = dialog;
        }

        public ObservableCollection<ReceptionGoodViewModel> ReceptionGoods
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
            _reception = new ReceptionWithExtraConfirmedGoods(
                new ReceptionWithUnkownGoods(
                    await parameters
                        .Value<ISupplier>("Supplier")
                        .Receptions.FirstAsync()
                )
            );
            ReceptionGoods = await _reception.NeedConfirmation().ToViewModelListAsync();
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            if (barcode.Symbology.ToLower() == "ean13")
            {
                var good = await _reception.ByBarcodeAsync(barcode.BarcodeData);
                var goodViewModel = ReceptionGoods.FirstOrDefault(x => x.Equals(good));
                if (goodViewModel != null)
                {
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    if (await good.ConfirmedAsync())
                    {
                        ReceptionGoods.Remove(goodViewModel);
                    }
                }
                else
                {
                    await _dialog.DisplayAlertAsync("Warning", "The item has already been scanned!", "Ok");
                    ReceptionGoods.Insert(0, new ReceptionGoodViewModel(good));
                }
            }
        }
    }
}
