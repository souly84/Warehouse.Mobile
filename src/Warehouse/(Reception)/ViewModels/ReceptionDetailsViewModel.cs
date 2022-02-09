using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediaPrint;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.Reception.ViewModels;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly INavigationService _navigationService;
        private readonly IKeyValueStorage _keyValueStorage;
        private ObservableCollection<ReceptionGroup>? _receptionGoods;
        private string? _itemCount;
        private int _originalCount;
        private string? _supplierName;
        private DelegateCommand? validateReceptionCommand;

        public ReceptionDetailsViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            INavigationService navigationService,
            IKeyValueStorage keyValueStorage)
            : base(scanner, dialog)
        {
            _navigationService = navigationService;
            _keyValueStorage = keyValueStorage;
        }

        public ObservableCollection<ReceptionGroup> ReceptionGoods
        {
            get => _receptionGoods ?? new ObservableCollection<ReceptionGroup>();
            set => SetProperty(ref _receptionGoods, value);
        }

        public string? ItemCount
        {
            get => _itemCount;
            set => SetProperty(ref _itemCount, value);
        }

        public string? SupplierName
        {
            get => _supplierName;
            set => SetProperty(ref _supplierName, value);
        }

        public DelegateCommand ValidateReceptionCommand => validateReceptionCommand ?? (validateReceptionCommand = new DelegateCommand(async () =>
        {
            try
            {
                foreach (var receptionGroup in ReceptionGoods)
                {
                    await receptionGroup.CommitAsync();
                }
                
                await _navigationService.ShowMessageAsync(PopupSeverity.Info, "Success!", "Your reception has been synchronized successfully.");
            }
            catch (Exception ex)
            {
                await _navigationService.ShowMessageAsync(PopupSeverity.Error, "Error!", "Synchronization failed. " + ex.Message);
            }
            await _navigationService.GoBackAsync();
        }));

        public Func<Task<bool>>? AnimateCounter { get; set; }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var supplier = parameters.Value<ISupplier>("Supplier");
            ReceptionGoods = await supplier.ReceptionViewModelsAsync(_keyValueStorage);
            _originalCount = ReceptionGoods.Sum(r => r.Count);
            SupplierName = supplier.ToDictionary().ValueOrDefault<string>("Name");
            RefreshCount();
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            if (barcode.Symbology.ToLower() == "ean13")
            {
                var good = await ReceptionGoods.ByBarcodeAsync(barcode.BarcodeData, true);
                var confirmed = await TryToConfirmGood(good);
                if (!confirmed)
                {
                    var goodViewModel = new ReceptionGoodViewModel(good);
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    ReceptionGoods.First().Insert(0, goodViewModel);
                    await ShowExtraGoodWarningMessageAsync(good);
                }
            }
        }

        private async Task<bool> TryToConfirmGood(IReceptionGood good)
        {
            foreach (var reception in ReceptionGoods)
            {
                var goodViewModel = reception.FirstOrDefault(x => x.Equals(good));
                if (goodViewModel != null)
                {
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    if (await good.ConfirmedAsync())
                    {
                        reception.Remove(goodViewModel);
                        RefreshCount();
                        if (AnimateCounter != null)
                        {
                            await AnimateCounter();
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private Task ShowExtraGoodWarningMessageAsync(IReceptionGood good)
        {
            if (good.IsUnknown)
            {
                return _navigationService.ShowMessageAsync(
                    PopupSeverity.Error,
                    "Error!",
                    "This item is not part of this delivery!"
                );
            }
            else
            {
                return _navigationService.ShowMessageAsync(
                    PopupSeverity.Warning,
                    "Warning!",
                    "This item has already been scanned"
                );
            }
        }

        private void RefreshCount()
        {
            var total = ReceptionGoods.Sum(receptionGroup => receptionGroup.Count(x => x.Regular));
            ItemCount = $"{total}/{_originalCount}";
        }
    }
}
