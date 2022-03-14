using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dotnet.Commands;
using EbSoft.Warehouse.SDK;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile
{
    public class StockMoveViewModel : ScannerViewModel
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _commands;

        public StockMoveViewModel(IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            ICommands commands,
            INavigationService navigationService) : base(scanner, dialog)
        {
            _scanner = scanner;
            _dialog = dialog;
            _company = company;
            _navigationService = navigationService;
            _commands = commands.Cached();
        }

        private string _scannedBarcodeOriginLocation;
        public string ScannedBarcodeOriginLocation
        {
            get => _scannedBarcodeOriginLocation;
            set => SetProperty(ref _scannedBarcodeOriginLocation, value);
        }

        private string _scannedProductToMove;
        public string ScannedProductToMove
        {
            get => _scannedProductToMove;
            set => SetProperty(ref _scannedProductToMove, value);
        }

        private string _scannedBarcodeDestinationLocation;
        public string ScannedBarcodeDestinationLocation
        {
            get => _scannedBarcodeDestinationLocation;
            set => SetProperty(ref _scannedBarcodeDestinationLocation, value);
        }

        private LocationViewModel _originLocationVm;
        public LocationViewModel OriginLocationVm
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

        private IWarehouseGood _warehouseGood;
        public IWarehouseGood WarehouseGood
        {
            get => _warehouseGood;
            set => SetProperty(ref _warehouseGood, value);
        }

        private ObservableCollection<LocationViewModel> _reserveLocations;
        public ObservableCollection<LocationViewModel> ReserveLocations
        {
            get => _reserveLocations;
            set => SetProperty(ref _reserveLocations, value);
        }

        private ObservableCollection<LocationViewModel> _raceLocations;
        public ObservableCollection<LocationViewModel> RaceLocations
        {
            get => _raceLocations;
            set => SetProperty(ref _raceLocations, value);
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            switch (barcode.Symbology.ToLower())
            {
                case "code128":
                    {
                        if (IsRecognizedProduct)
                        {
                            await _navigationService.NavigateAsync(
                                     AppConstants.QuantityToMovePopupViewId,
                                     new NavigationParameters
                                     {
                                        { "Origin", OriginLocationVm.ToStorage() },
                                        { "Destination", barcode.BarcodeData },
                                        { "Good", WarehouseGood }
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
                            WarehouseGood = await _company
                                .Warehouse
                                .Goods.For(barcode.BarcodeData)
                                .FirstAsync();
                            var storage = await WarehouseGood.Storages.ByBarcodeAsync(ScannedBarcodeOriginLocation);
                            OriginLocationVm = new LocationViewModel(storage);
                            RaceLocations = (ObservableCollection<LocationViewModel>)await WarehouseGood.Storages.Race.ToViewModelListAsync();
                            ReserveLocations = (ObservableCollection<LocationViewModel>)await WarehouseGood.Storages.Reserve.ToViewModelListAsync();
                        }
                        else
                        {
                            await _dialog.DisplayAlertAsync("Error", "Please scan the location you wants to move from before scanning a product", "Ok");
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

        public IAsyncCommand BackCommand => _commands.AsyncCommand(async () =>
        {
            await _navigationService.GoBackAsync();
        });
    }
}