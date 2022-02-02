using System;
using MediaPrint;
using Prism.Commands;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class SupplierViewModel
    {
        private readonly ISupplier _supplier;
        private readonly INavigationService _navigationService;
        private DelegateCommand? goToReceptionDetailsCommand;

        public SupplierViewModel(
            ISupplier supplier,
            INavigationService navigationService)
        {
            _supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public string Name => _supplier.ToDictionary().ValueOrDefault<string>("Name");

        public DelegateCommand GoToReceptionDetailsCommand => goToReceptionDetailsCommand ?? (goToReceptionDetailsCommand = new DelegateCommand(() =>
            _navigationService.NavigateAsync(
                AppConstants.ReceptionDetailsViewId,
                new NavigationParameters
                {
                    { "Supplier", _supplier }
                }
            )
        ));
    }
}
