using System;
using Dotnet.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile
{
    public class QuantityToMovePopupViewModel : BindableBase, IInitialize
    {
        private readonly INavigationService _navigationService;
        private readonly ICompany _company;
        private IWarehouseGood? _goodToMove;
        private readonly CachedCommands _commands;

        public QuantityToMovePopupViewModel(
            ICommands commands,
            INavigationService navigationService,
            ICompany company)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _company = company;
            _commands = commands.Cached();
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

        private int _quantityToMove = 1;
        public int QuantityToMove
        {
            get => _quantityToMove;
            set => SetProperty(ref _quantityToMove, value);
        }

        public IAsyncCommand ValidateCommand => _commands.AsyncCommand(async () =>
        {
            _ = _goodToMove ?? throw new InvalidOperationException($"Good object is not initialized for movement");
            _ = OriginLocation ?? throw new InvalidOperationException($"Origin location value is not initialized for movement");
            await _goodToMove
                .Movement
                .From(OriginLocation)
                .MoveToAsync(
                    await _goodToMove.Storages.ByBarcodeAsync(_company.Warehouse, DestinationLocation),
                    QuantityToMove
            );
            await _navigationService.GoBackAsync();
        });

        public IAsyncCommand CancelCommand => _commands.AsyncCommand(() =>
            _navigationService.GoBackAsync()
        );

        public void Initialize(INavigationParameters parameters)
        {
            OriginLocation = parameters.Value<IStorage>("Origin");
            DestinationLocation = parameters.Value<string>("Destination");
            _goodToMove = parameters.Value<IWarehouseGood>("Good");
        }
    }
}
