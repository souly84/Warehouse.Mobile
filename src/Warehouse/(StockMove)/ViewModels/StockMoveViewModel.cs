using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile
{
    public class StockMoveViewModel : BindableBase, IInitializeAsync, INavigatedAware
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;

        public StockMoveViewModel(IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            INavigationService navigationService)
        {
            _scanner = scanner;
            _dialog = dialog;
            _company = company;
            _navigationService = navigationService;
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

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            
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

        protected virtual async void OnScan(object sender, IScanningResult barcode)
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
                                        { "Origin", OriginLocationVm },
                                        { "Destination", barcode.BarcodeData },
                                        { "Good", WarehouseGood }
                                     }
                                 );
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
                        break;
                    }
            }
        }
    }
}
