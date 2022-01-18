using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using MediaPrint;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile
{
    public class PutAwayViewModel : ScannerViewModel, IInitializeAsync
    {
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;

        public PutAwayViewModel(
            IScanner scanner,
            IPageDialogService dialog,
            ICompany company,
            INavigationService navigationService)
            : base(scanner, dialog)
        {
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

        public Task InitializeAsync(INavigationParameters parameters)
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

            return Task.CompletedTask;
        }

        protected override async Task OnScanAsync(IScanningResult barcode)
        {
            ScannedBarcode = barcode.BarcodeData;
            IsRecognizedProduct = true;
            WarehouseGood = await _company
                .Warehouse
                .Goods.For(barcode.BarcodeData)
                .FirstAsync();
            CheckInStorage = await WarehouseGood
                .Storages
                .PutAway.FirstAsync();
            CheckInQuantity = CheckInStorage.ToDictionary().ValueOrDefault<int>("Quantity");
        }
    }
}
