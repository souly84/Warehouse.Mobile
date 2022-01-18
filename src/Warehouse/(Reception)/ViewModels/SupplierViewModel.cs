using System;
using System.Collections.Generic;
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

        public SupplierViewModel(
            ISupplier supplier,
            INavigationService navigationService)
        {
            _supplier = supplier;
            _navigationService = navigationService;
        }

        public string Name { get => ((IPrintable)_supplier).ToDictionary().ValueOrDefault<string>("Name"); }

        private DelegateCommand goToReceptionDetailsCommand;

        public DelegateCommand GoToReceptionDetailsCommand => goToReceptionDetailsCommand ?? (goToReceptionDetailsCommand = new DelegateCommand(() =>
            _navigationService.NavigateAsync(
                AppConstants.ReceptionDetailsViewId,
                new NavigationParameters { { "Supplier", _supplier } }
            )
        ));
    }
}
