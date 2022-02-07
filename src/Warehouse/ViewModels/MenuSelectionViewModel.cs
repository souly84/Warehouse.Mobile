using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Mobile.Helper;

namespace Warehouse.Mobile.ViewModels
{
    public class MenuSelectionViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private ICommand? goToAvailableSuppliersCommand;
        private ICommand? goToPutAwayCommand;

        private string buildVersion;
        public string BuildVersion
        {
            get { return buildVersion; }
            set { SetProperty(ref buildVersion, value); }
        }

        public MenuSelectionViewModel(INavigationService navigationService, IDeviceHelper helper)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            BuildVersion = helper.GetBuildVersion();
        }

        public ICommand GoToAvailableSuppliersCommand => goToAvailableSuppliersCommand ?? (goToAvailableSuppliersCommand = new DelegateCommand(() =>
            _navigationService.NavigateAsync(AppConstants.SupplierViewId)
        ));

        public ICommand GoToPutAwayCommand => goToPutAwayCommand ?? (goToPutAwayCommand = new DelegateCommand(() =>
            _navigationService.NavigateAsync(AppConstants.PutAwayViewId)
        ));
    }
}
