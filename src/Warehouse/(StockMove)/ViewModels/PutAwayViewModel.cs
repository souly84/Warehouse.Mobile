using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile
{
    public class PutAwayViewModel : ScannerViewModel
    {
        private readonly IPageDialogService _dialog;
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _commands;

        public PutAwayViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            ICommands commands,
            INavigationService navigationService)
            : base(scanner, dialog)
        {
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _company = company ?? throw new ArgumentNullException(nameof(company));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _commands = commands.Cached();
        }

        private ObservableCollection<LocationViewModel>? _reserveLocations;
        public ObservableCollection<LocationViewModel> ReserveLocations
        {
            get => _reserveLocations ?? new ObservableCollection<LocationViewModel>();
            set => SetProperty(ref _reserveLocations, value);
        }

        private ObservableCollection<LocationViewModel>? _raceLocations;
        public ObservableCollection<LocationViewModel> RaceLocations
        {
            get => _raceLocations ?? new ObservableCollection<LocationViewModel>();
            set => SetProperty(ref _raceLocations, value);
        }

        private string _scannedBarcode = string.Empty;
        public string ScannedBarcode
        {
            get => _scannedBarcode;
            set => SetProperty(ref _scannedBarcode, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private string _destinationBarcode = string.Empty;
        public string DestinationBarcode
        {
            get => _destinationBarcode;
            set => SetProperty(ref _destinationBarcode, value);
        }

        private bool _isRecognizedProduct;
        public bool IsRecognizedProduct
        {
            get => _isRecognizedProduct;
            set => SetProperty(ref _isRecognizedProduct, value);
        }

        private IWarehouseGood? _warehouseGood;
        public IWarehouseGood? WarehouseGood
        {
            get => _warehouseGood;
            set => SetProperty(ref _warehouseGood, value);
        }

        private int _checkInQuantity;
        public int CheckInQuantity
        {
            get => _checkInQuantity;
            set => SetProperty(ref _checkInQuantity, value);
        }

        private LocationViewModel? _putAwayStorage;
        public LocationViewModel? PutAwayStorage
        {
            get => _putAwayStorage;
            set => SetProperty(ref _putAwayStorage, value);
        }

        public IAsyncCommand GoToPopupCommand => _commands.NavigationCommand(() =>
            _navigationService.NavigateAsync(
                AppConstants.QuantityToMovePopupViewId,
                new NavigationParameters
                {
                    // For now we have to mock params here
                    { "Origin", new MockStorage("MockStorage") },
                    { "Destination", "Mock Destination" },
                    { "Good", new MockWarehouseGood("1", 2) },
                }
            )
        );

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            switch (barcode.Symbology.ToLower())
            {
                case "code128":
                {
                    DestinationBarcode = barcode.BarcodeData;
                    _ = PutAwayStorage ?? throw new InvalidOperationException($"{nameof(PutAwayStorage)} is not initialized");
                    _ = WarehouseGood ?? throw new InvalidOperationException($"{nameof(WarehouseGood)} is not initialized");
                    if (CheckInQuantity > 1)
                    {
                        await _navigationService.NavigateAsync(
                            AppConstants.QuantityToMovePopupViewId,
                            new NavigationParameters
                            {
                                { "Origin", PutAwayStorage.ToStorage() },
                                { "Destination", barcode.BarcodeData },
                                { "Good", WarehouseGood }
                            }
                        );
                    }
                    else
                    {
                        await WarehouseGood
                            .Movement
                            .From(PutAwayStorage.ToStorage())
                            .MoveToAsync(
                                await WarehouseGood.Storages.ByBarcodeAsync(_company.Warehouse, barcode.BarcodeData),
                                1
                            );
                    }

                    StatusMessage = "Item successfully assigned";
                    ResetFields();
                    break;
                }
                default:
                {
                    ScannedBarcode = barcode.BarcodeData;
                    WarehouseGood = await _company
                        .Warehouse
                        .Goods
                        .FirstAsync(barcode.BarcodeData);

                    var checkIn = await WarehouseGood.Storages.PutAway.ToViewModelListAsync();
                    if (!checkIn.Any())
                    {
                        ResetFields();
                        ScannedBarcode = barcode.BarcodeData;
                        WarehouseGood = await _company
                            .Warehouse
                            .Goods
                            .FirstAsync(barcode.BarcodeData);

                        if (!checkIn.Any())
                        {
                            await _dialog.ErrorAsync(
                                "This item is not present in the check in area"
                            );
                            return;
                        }
                        
                        break;
                    }
                    IsRecognizedProduct = true;
                    PutAwayStorage = checkIn.First();
                    CheckInQuantity = await PutAwayStorage.ToStorage().QuantityForAsync(WarehouseGood);
                    RaceLocations = await WarehouseGood.Storages.Race.ToViewModelListAsync();
                    ReserveLocations = await WarehouseGood.Storages.Reserve.ToViewModelListAsync();
                    break;
                }
            }
        }

        private void ResetFields()
        {
            ScannedBarcode = "";
            DestinationBarcode = "";
            IsRecognizedProduct = false;
            PutAwayStorage = null;
        }

        public IAsyncCommand BackCommand => _commands.AsyncCommand(_navigationService.GoBackAsync);
    }
}
