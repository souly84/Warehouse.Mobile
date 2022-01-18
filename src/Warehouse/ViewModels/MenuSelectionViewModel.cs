using System;
<<<<<<< HEAD
using System.Threading.Tasks;
=======
using System.Windows.Input;
>>>>>>> main
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Warehouse.Mobile.ViewModels
{
    public class MenuSelectionViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private ICommand goToAvailableSuppliersCommand;
        private ICommand goToPutAwayCommand;

        public MenuSelectionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public ICommand GoToAvailableSuppliersCommand => goToAvailableSuppliersCommand ?? (goToAvailableSuppliersCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(AppConstants.SupplierViewId);
        }));

        public ICommand GoToPutAwayCommand => goToPutAwayCommand ?? (goToPutAwayCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(AppConstants.PutAwayViewId);
        }));
    }
}
