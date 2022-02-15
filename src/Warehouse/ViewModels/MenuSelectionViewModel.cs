using System;
using System.Windows.Input;
using Dotnet.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Mobile.ViewModels
{
    public class MenuSelectionViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly CachedCommands _commands;

        public MenuSelectionViewModel(
            ICommands commands,
            INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _commands = commands.Cached();
        }

        public ICommand GoToAvailableSuppliersCommand => _commands.NavigationCommand(() =>
             _navigationService.NavigateAsync(AppConstants.SupplierViewId)
        );

        public IAsyncCommand GoToPutAwayCommand => _commands.NavigationCommand(() =>
            _navigationService.NavigateAsync(AppConstants.PutAwayViewId)
        );
    }
}
