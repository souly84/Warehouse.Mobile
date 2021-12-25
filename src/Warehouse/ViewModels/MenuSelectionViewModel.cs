using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Warehouse.Mobile.ViewModels
{
    public class MenuSelectionViewModel : BindableBase, IInitializeAsync
    {
        private readonly INavigationService _navigationService;

        public MenuSelectionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


     

        public async Task InitializeAsync(INavigationParameters parameters)
        {
        }

        private DelegateCommand goToAvailableSuppliersCommand;

        public DelegateCommand GoToAvailableSuppliersCommand => goToAvailableSuppliersCommand ?? (goToAvailableSuppliersCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(AppConstants.SupplierViewId);
        }));

        private DelegateCommand goToPutAwayCommand;

        public DelegateCommand GoToPutAwayCommand => goToPutAwayCommand ?? (goToPutAwayCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(AppConstants.PutAwayViewId);
        }));

        private DelegateCommand goToStockMoveCommand;

        public DelegateCommand GoToStockMoveCommand => goToStockMoveCommand ?? (goToStockMoveCommand = new DelegateCommand(async () =>
        {
            await _navigationService.NavigateAsync(AppConstants.StockMoveViewId);
        }));

    }
}
