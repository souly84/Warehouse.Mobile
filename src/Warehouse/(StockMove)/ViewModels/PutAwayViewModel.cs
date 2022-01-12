using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using MediaPrint;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;

namespace Warehouse.Mobile
{
    public class PutAwayViewModel : BindableBase, IInitializeAsync, INavigatedAware
    {
        private readonly IScanner _scanner;
        private readonly IPageDialogService _dialog;
        private readonly ICompany _company;

        public PutAwayViewModel(IScanner scanner, IPageDialogService dialog, ICompany company)
        {
            _scanner = scanner;
            _dialog = dialog;
            _company = company;
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

        private IStorage _checkInStorage;
        public IStorage CheckInStorage
        {
            get => _checkInStorage;
            set => SetProperty(ref _checkInStorage, value);
        }

        private string _scannedBarcode;
        public string ScannedBarcode
        {
            get => _scannedBarcode;
            set => SetProperty(ref _scannedBarcode, value);
        }

        private bool _isRecognizedProduct;
        public bool IsRecognizedProduct
        {
            get => _isRecognizedProduct;
            set => SetProperty(ref _isRecognizedProduct, value);
        }

        private IWarehouseGood _warehouseGood;
        public IWarehouseGood WarehouseGood
        {
            get => _warehouseGood;
            set => SetProperty(ref _warehouseGood, value);
        }

        private int? _checkInQuantity;
        public int? CheckInQuantity
        {
            get => _checkInQuantity;
            set => SetProperty(ref _checkInQuantity, value);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            ReserveLocations = new ObservableCollection<LocationViewModel>
            {
                new LocationViewModel
                {
                    Location = "41-1-3", LocationaType = LocationType.Reserve
                },
                new LocationViewModel
                {
                    Location = "42-1-2", LocationaType = LocationType.Reserve
                }
            };

            RaceLocations = new ObservableCollection<LocationViewModel>
            {
                new LocationViewModel
                {
                    Location = "41-1-1", LocationaType = LocationType.Race
                },
                new LocationViewModel
                {
                    Location = "42-1-1", LocationaType = LocationType.Race
                }
            };
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
                    ScannedBarcode = barcode.BarcodeData;
                    IsRecognizedProduct = true;
                    WarehouseGood = await _company
                        .Warehouse
                        .Goods.For(barcode.BarcodeData)
                        .FirstAsync();
                    CheckInStorage = await WarehouseGood?.Storages?
                        .FirstAsync(x => x.ToDictionary().ValueOrDefault<string>("location")
                        .Contains("CHECK IN"));
                    CheckInQuantity = CheckInStorage?.ToDictionary().ValueOrDefault<int>("Quantity");

                }
                catch (Exception ex)
                {
                    await _dialog.DisplayAlertAsync("Error scanning", ex.Message, "ok");
                }
            });
        }
    }
}
