using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly IPageDialogService _dialog;
        private ReceptionWithUnkownGoods _reception;
        private IList<ReceptionGoodViewModel> _receptionGoods;
        private DelegateCommand validateReceptionCommand;

        public ReceptionDetailsViewModel(IScanner scanner, IPageDialogService dialog)
            : base(scanner, dialog)
        {
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

        protected override async Task OnScanAsync(IScanningResult barcode)
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
    }
}
