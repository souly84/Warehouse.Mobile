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

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly INavigationService _navigationService;
        private readonly IKeyValueStorage _keyValueStorage;
        private IReception? _reception;
        private ObservableCollection<ReceptionGoodViewModel>? _receptionGoods;
        private string? _itemCount;
        private string? _originalCount;
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

        public ObservableCollection<ReceptionGoodViewModel> ReceptionGoods
        {
            get => _receptionGoods ?? new ObservableCollection<ReceptionGoodViewModel>();
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
                _ = _reception ?? throw new InvalidOperationException($"Reception object is not initialized");
                await _reception.Confirmation().CommitAsync();
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
            var supplierReception = await parameters
                .Value<ISupplier>("Supplier")
                .Receptions.FirstAsync();

            _reception = supplierReception
                .WithExtraConfirmed()
                .WithoutInitiallyConfirmed()
                .WithConfirmationProgress(_keyValueStorage);

            ReceptionGoods = await _reception
                .NotConfirmedOnly()
                .ToViewModelListAsync();
            _originalCount = ReceptionGoods.Count.ToString();
            SupplierName = ((IPrintable)parameters.Value<ISupplier>("Supplier")).ToDictionary().ValueOrDefault<string>("Name");
            RefreshCount();
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            if (barcode.Symbology.ToLower() == "ean13")
            {
                _ = _reception ?? throw new InvalidOperationException($"Reception object is not initialized");
                var good = (await _reception.ByBarcodeAsync(barcode.BarcodeData, true)).First();
                var goodViewModel = ReceptionGoods.FirstOrDefault(x => x.Equals(good));
                if (goodViewModel != null)
                {
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    if (await good.ConfirmedAsync())
                    {
                        ReceptionGoods.Remove(goodViewModel);
                        RefreshCount();
                        if (AnimateCounter != null)
                        {
                            await AnimateCounter();
                        }
                    }
                }
                else
                {
                    goodViewModel = new ReceptionGoodViewModel(good);
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    ReceptionGoods.Insert(0, goodViewModel);
                    await ShowExtraGoodWarningMessageAsync(good);
                }
            }
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
            ItemCount = $"{ReceptionGoods.Count(x => !x.IsExtraConfirmedReceptionGood && !x.IsUnkownGood) }/{_originalCount}";
        }
    }
}
