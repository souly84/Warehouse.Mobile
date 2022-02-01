using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile
{
    public class QuantityToMovePopupViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private IWarehouseGood? _goodToMove;

        public QuantityToMovePopupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        private IStorage? _originLocation;
        public IStorage? OriginLocation
        {
            get => _originLocation;
            set => SetProperty(ref _originLocation, value);
        }

        private string _destinationLocation = string.Empty;
        public string DestinationLocation
        {
            get => _destinationLocation;
            set => SetProperty(ref _destinationLocation, value);
        }

        public void Initialize(INavigationParameters parameters)
        {
            OriginLocation = parameters.Value<IStorage>("Origin");
            DestinationLocation = parameters.Value<string>("Destination");
            _goodToMove = parameters.Value<IWarehouseGood>("Good");
        }

        private DelegateCommand? validateCommand;

        public DelegateCommand ValidateCommand => validateCommand ?? (validateCommand = new DelegateCommand(async () =>
        {
            _ = _goodToMove ?? throw new InvalidOperationException($"Good object is not initialized for movement");
            _ = OriginLocation ?? throw new InvalidOperationException($"Origin location value is not initialized for movement");
            await _goodToMove
                .Movement
                .From(OriginLocation)
                .MoveToAsync(
                    await _goodToMove.Storages.ByBarcodeAsync(DestinationLocation),
                    5
            );
            await _navigationService.GoBackAsync();
        }));

        private DelegateCommand? cancelCommand;
        public DelegateCommand CancelCommand => cancelCommand ?? (cancelCommand = new DelegateCommand(async () =>
        {
            await _navigationService.GoBackAsync();
        }));
    }
}
