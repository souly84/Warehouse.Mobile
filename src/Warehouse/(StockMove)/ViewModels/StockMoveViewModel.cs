using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile.ViewModels
{
    public class StockMoveViewModel : ScannerViewModel
    {
        private readonly IPageDialogService _dialog;
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _commands;
        private IWarehouseGood? _warehouseGood;

        public StockMoveViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            ICommands commands,
            INavigationService navigationService)
            : base(scanner, dialog)
        {
            _dialog = dialog;
            _company = company;
            _navigationService = navigationService;
            _commands = commands.Cached();
        }

        private string? _scannedBarcodeOriginLocation;
        public string? ScannedBarcodeOriginLocation
        {
            get => _scannedBarcodeOriginLocation;
            set => SetProperty(ref _scannedBarcodeOriginLocation, value);
        }

        private string? _scannedProductToMove;
        public string? ScannedProductToMove
        {
            get => _scannedProductToMove;
            set => SetProperty(ref _scannedProductToMove, value);
        }

        private string? _scannedBarcodeDestinationLocation;
        public string? ScannedBarcodeDestinationLocation
        {
            get => _scannedBarcodeDestinationLocation;
            set => SetProperty(ref _scannedBarcodeDestinationLocation, value);
        }

        private LocationViewModel? _originLocationVm;
        public LocationViewModel? OriginLocationVm
        {
            get => _originLocationVm;
            set => SetProperty(ref _originLocationVm, value);
        }

        private bool _isRecognizedProduct;
        public bool IsRecognizedProduct
        {
            get => _isRecognizedProduct;
            set => SetProperty(ref _isRecognizedProduct, value);
        }

        private bool _isRecognizedOriginLocation;
        public bool IsRecognizedOriginLocation
        {
            get => _isRecognizedOriginLocation;
            set => SetProperty(ref _isRecognizedOriginLocation, value);
        }

        private ObservableCollection<LocationViewModel>? _reserveLocations;
        public ObservableCollection<LocationViewModel>? ReserveLocations
        {
            get => _reserveLocations;
            set => SetProperty(ref _reserveLocations, value);
        }

        private ObservableCollection<LocationViewModel>? _raceLocations;
        public ObservableCollection<LocationViewModel>? RaceLocations
        {
            get => _raceLocations;
            set => SetProperty(ref _raceLocations, value);
        }

        public IAsyncCommand BackCommand => _commands.AsyncCommand(() => _navigationService.GoBackAsync());

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            switch (barcode.Symbology.ToLower())
            {
                case "code128":
                    {
                        if (IsRecognizedProduct)
                        {
                            _ = OriginLocationVm ?? throw new InvalidOperationException(
                                "QuantityToMovePopup navigation error. Original location is not specified."
                            );
                            await _navigationService.NavigateAsync(
                                AppConstants.QuantityToMovePopupViewId,
                                new NavigationParameters
                                {
                                   { "Origin", OriginLocationVm.ToStorage() },
                                   { "Destination", barcode.BarcodeData },
                                   { "Good", _warehouseGood }
                                }
                            );
                            ResetFields();
                            break;
                        }
                        else
                        {
                            ScannedBarcodeOriginLocation = barcode.BarcodeData;
                            IsRecognizedOriginLocation = true;
                        }
                        break;
                    }
                default:
                    {
                        if (IsRecognizedOriginLocation)
                        {
                            ScannedProductToMove = barcode.BarcodeData;
                            IsRecognizedProduct = true;
                            _warehouseGood = await _company
                                .Warehouse
                                .Goods.FirstAsync(barcode.BarcodeData);
                            var storage = await _warehouseGood
                                .Storages
                                .ByBarcodeAsync(ScannedBarcodeOriginLocation ?? string.Empty);
                            OriginLocationVm = new LocationViewModel(storage);
                            RaceLocations = await _warehouseGood.Storages.Race.ToViewModelListAsync();
                            ReserveLocations = await _warehouseGood.Storages.Reserve.ToViewModelListAsync();
                        }
                        else
                        {
                            await _dialog.ErrorAsync(
                                "Please scan the location you wants to move from before scanning a product"
                            );
                        }
                        break;
                    }
            }
        }

        private void ResetFields()
        {
            IsRecognizedOriginLocation = false;
            IsRecognizedProduct = false;
            ScannedProductToMove = "";
            ScannedBarcodeOriginLocation = "";
        }
    }
}
