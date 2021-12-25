using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Prism.Commands;
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
        private readonly INavigationService _navigationService;

        public PutAwayViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            INavigationService navigationService)
        {
            _scanner = scanner;
            _dialog = dialog;
            _company = company;
            _navigationService = navigationService;
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

        private LocationViewModel _putAwayStorage;
        public LocationViewModel PutAwayStorage
        {
            get => _putAwayStorage;
            set => SetProperty(ref _putAwayStorage, value);
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
            //ReserveLocations = new ObservableCollection<LocationViewModel>
            //{
            //    new LocationViewModel
            //    {
            //        Location = "41-1-3", LocationaType = LocationType.Reserve
            //    },
            //    new LocationViewModel
            //    {
            //        Location = "42-1-2", LocationaType = LocationType.Reserve
            //    }
            //};

            //RaceLocations = new ObservableCollection<LocationViewModel>
            //{
            //    new LocationViewModel
            //    {
            //        Location = "41-1-1", LocationaType = LocationType.Race
            //    },
            //    new LocationViewModel
            //    {
            //        Location = "42-1-1", LocationaType = LocationType.Race
            //    }
            //};
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
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    switch (barcode.Symbology.ToLower())
                    {
                        case "code128":
                            {
                                await _navigationService.NavigateAsync(
                                    AppConstants.QuantityToMovePopupViewId,
                                    new NavigationParameters
                                    {
                                        { "Origin", PutAwayStorage },
                                        { "Destination", barcode.BarcodeData },
                                        { "Good", WarehouseGood }
                                    }
                                );
                                break;
                            }
                        default:
                            {
                                ScannedBarcode = barcode.BarcodeData;
                                IsRecognizedProduct = true;
                                WarehouseGood = await _company
                                    .Warehouse
                                    .Goods.For(barcode.BarcodeData)
                                    .FirstAsync();

                                var check = await WarehouseGood.Storages.PutAway.ToListAsync();
                                //PutAwayStorage = await check.FirstAsync();
                                RaceLocations = (ObservableCollection<LocationViewModel>)await WarehouseGood.Storages.Race.ToViewModelListAsync();
                                ReserveLocations = (ObservableCollection<LocationViewModel>)await WarehouseGood.Storages.Reserve.ToViewModelListAsync();
                                break;
                            }
                    }
                    

                }
                catch (Exception ex)
                {
                    await _dialog.DisplayAlertAsync("Error scanning", ex.Message, "ok");
                }
            });
        }

        private DelegateCommand goToPopupCommand;

        public DelegateCommand GoToPopupCommand => goToPopupCommand ?? (goToPopupCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(
                       AppConstants.QuantityToMovePopupViewId);
        }));

        private DelegateCommand searchItemCommand;

        public DelegateCommand SearchItemCommand => searchItemCommand ?? (searchItemCommand = new DelegateCommand(async () =>
        {
            SearchItem(ScannedBarcode);
        }));
    }
}
