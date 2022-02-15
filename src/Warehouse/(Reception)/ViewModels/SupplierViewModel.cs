using System;
using Dotnet.Commands;
using MediaPrint;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class SupplierViewModel
    {
        private readonly ISupplier _supplier;
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _commands;

        public SupplierViewModel(
            ISupplier supplier,
            ICommands commands,
            INavigationService navigationService)
        {
            _supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _ = commands ?? throw new ArgumentNullException(nameof(commands));
            _commands = commands.Cached();
        }

        public string Name => _supplier.ToDictionary().ValueOrDefault<string>("Name");

        public IAsyncCommand GoToReceptionDetailsCommand => _commands.NavigationCommand(() =>
            _navigationService.NavigateAsync(
                AppConstants.ReceptionDetailsViewId,
                new NavigationParameters
                {
                    { "Supplier", _supplier }
                }
            )
        );
    }
}
