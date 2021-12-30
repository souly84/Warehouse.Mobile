using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Extensions;
using Xamarin.Forms;

namespace Warehouse.Mobile
{
    public class QuantityToMovePopupViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private readonly ICompany _company;
        private IWarehouseGood _goodToMove;


        public QuantityToMovePopupViewModel(INavigationService navigationService, ICompany company)
        {
            _navigationService = navigationService;
            _company = company;
        }

        private LocationViewModel _originLocation;
        public LocationViewModel OriginLocation
        {
            get => _originLocation;
            set => SetProperty(ref _originLocation, value);
        }

        private string _destinationLocation;
        public string DestinationLocation
        {
            get => _destinationLocation;
            set => SetProperty(ref _destinationLocation, value);
        }

        


        public void Initialize(INavigationParameters parameters)
        {
            OriginLocation = parameters.Value<LocationViewModel>("Origin");
            DestinationLocation = parameters.Value<string>("Destination");
            _goodToMove = parameters.Value<IWarehouseGood>("Good");
        }

        private DelegateCommand validateCommand;

        public DelegateCommand ValidateCommand => validateCommand ?? (validateCommand = new DelegateCommand(async () =>
        {

            await _goodToMove
                .Movement
                .From(OriginLocation.ToStorage())
                .MoveToAsync(
                    await _goodToMove.Storages.ByBarcodeAsync(_company.Warehouse, DestinationLocation),
                    1
            );
            await _navigationService.GoBackAsync();
        }));

        private DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand => cancelCommand ?? (cancelCommand = new DelegateCommand(async () =>
        {
            await _navigationService.GoBackAsync();
        }));
    }
}
