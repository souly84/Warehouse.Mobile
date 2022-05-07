using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using MediaPrint;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _cachedCommands;
        private readonly ICommands _commands;
        private readonly IKeyValueStorage _keyValueStorage;
        private ObservableCollection<ReceptionGroup>? _receptionGoods;
        private string? _itemCount;
        private int _originalCount;
        private string? _supplierName;
        private ISupplier? _supplier;

        public ReceptionDetailsViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            INavigationService navigationService,
            ICommands commands,
            IKeyValueStorage keyValueStorage)
            : base(scanner, dialog)
        {
            _scanner = scanner;
            _dialog = dialog;
            _navigationService = navigationService;
            _commands = commands;
            _cachedCommands = commands.Cached();
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

        public Func<Task<bool>>? AnimateCounter { get; set; }

        public IAsyncCommand ValidateReceptionCommand => _cachedCommands.AsyncCommand(async () =>
        {
            try
            {
                foreach (var receptionGroup in ReceptionGoods)
                {
                    await receptionGroup.CommitAsync();
                }
                
                await _navigationService.ShowSuccessAsync(
                    "Your reception has been synchronized successfully."
                );
            }
            catch (Exception ex)
            {
                await _navigationService.ShowErrorAsync(
                    "Synchronization failed. " + ex.Message
                );
            }
            await _navigationService.GoBackAsync();
        });

        public IAsyncCommand GoToHistoryCommand => _cachedCommands.NavigationCommand(() =>
            _navigationService.NavigateAsync(
                AppConstants.HistoryViewId,
                new NavigationParameters { { "Supplier", _supplier } }
            )
        );

        public IAsyncCommand BackCommand => _cachedCommands.AsyncCommand(async () =>
        {
            try
            {
                if (await _dialog.WarningAsync("Are you sure you want to leave this reception?"))
                {
                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await _navigationService.ShowErrorAsync(
                    "Synchronization failed. " + ex.Message
                );
            }
        });

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            try
            {
                _supplier = parameters.Value<ISupplier>("Supplier");
                SupplierName = _supplier.ToDictionary().ValueOrDefault<string>("Name");
                ReceptionGoods = await _supplier.ReceptionViewModelsAsync(_commands, _keyValueStorage);
                _originalCount = ReceptionGoods.TotalQuantity();
                RefreshCount();
            }
            catch (Exception ex)
            {
                await _dialog.ErrorAsync(ex);
                throw;
            }
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            if (barcode.Symbology.ToLower() == "ean13")
            {
                var good = await ReceptionGoods.ByBarcodeAsync(barcode.BarcodeData, true);
                var confirmed = await ReceptionGoods.TryToConfirmGood(good);
                if (!confirmed)
                {
                    var goodViewModel = new ReceptionGoodViewModel(good, _commands);
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    var reception = await ReceptionGoods.GoodReceptionAsync(good, barcode.BarcodeData);
                    reception.Insert(0, goodViewModel);
                    _scanner.BeepFailure();
                    await ShowExtraGoodWarningMessageAsync(good);
                }
                else
                {
                    _scanner.BeepSuccess();
                    RefreshCount();
                    if (AnimateCounter != null)
                    {
                        await AnimateCounter();
                    }
                }
            }
            else
            {
                _scanner.BeepFailure();
                await _navigationService.ShowErrorAsync("This barcode type is not supported");
            }
        }

        private Task ShowExtraGoodWarningMessageAsync(IReceptionGood good)
        {
            _scanner.BeepFailure();
            if (good.IsUnknown)
            {
                return _navigationService.ShowErrorAsync(
                    "This item is not part of this delivery!"
                );
            }
            else
            {
                return _navigationService.ShowWarningAsync(
                    "This item has already been scanned"
                );
            }
        }

        private void RefreshCount()
        {
            ItemCount = $"{ReceptionGoods.RemainToConfrim()}/{_originalCount}";
        }
    }
}
