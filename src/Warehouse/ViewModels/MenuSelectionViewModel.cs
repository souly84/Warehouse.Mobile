using System;
using Dotnet.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.Interfaces;

namespace Warehouse.Mobile.ViewModels
{
    public class MenuSelectionViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEnvironment _environment;
        private readonly CachedCommands _commands;
        private readonly IPageDialogService _dialog;

        public MenuSelectionViewModel(
            IPageDialogService dialog,
            INavigationService navigationService,
            ICommands commands,
            IEnvironment environment)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _commands = commands?.Cached() ?? throw new ArgumentNullException(nameof(commands));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
        }

        public IAsyncCommand GoToAvailableSuppliersCommand => _commands.NavigationCommand(() =>
             _navigationService.NavigateAsync(AppConstants.SupplierViewId)
        );

        public IAsyncCommand GoToPutAwayCommand => _commands.NavigationCommand(() =>
            _navigationService.NavigateAsync(AppConstants.PutAwayViewId)
        );

        public IAsyncCommand GoToStockMoveCommand => _commands.NavigationCommand(() =>
            _navigationService.NavigateAsync(AppConstants.StockMoveViewId)
        );

        public IAsyncCommand BackCommand => _commands.AsyncCommand(async () =>
        {
            if (await _dialog.WarningAsync("Are you sure you want to quit the application?"))
            {
                _environment.ExitApp();
            }
        });
    }
}
