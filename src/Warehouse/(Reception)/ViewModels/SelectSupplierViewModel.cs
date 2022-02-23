using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Commands;
using EbSoft.Warehouse.SDK;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class SelectSupplierViewModel : BindableBase, IInitializeAsync
    {
        private readonly ICompany _company;
        private readonly IOverlay _overlay;
        private readonly CachedCommands _cachedCommands;
        private readonly ICommands _commands;
        private readonly INavigationService _navigationService;

        public SelectSupplierViewModel(
            ICompany company,
            IOverlay overlay,
            ICommands commands,
            INavigationService navigationService)
        {
            _company = company ?? throw new ArgumentNullException(nameof(company));
            _overlay = overlay ?? throw new ArgumentNullException(nameof(overlay));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _cachedCommands = commands.Cached();
        }

        private IList<SupplierViewModel>? _suppliers;
        public IList<SupplierViewModel> Suppliers
        {
            get => _suppliers ?? new List<SupplierViewModel>();
            set => SetProperty(ref _suppliers, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        public IAsyncCommand ChangeSelectedDateCommand => _cachedCommands.AsyncCommand(() => RefreshAvailableSupplierList());

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            SelectedDate = DateTime.Now;
        }

        private Task RefreshAvailableSupplierList()
        {
            return _overlay.OverlayAsync(async () =>
                Suppliers = await _company
                    .Suppliers
                    .For(SelectedDate)
                    .ToViewModelListAsync(
                        _navigationService,
                        _overlay,
                        _commands
                    ),
                "Suppliers Loading..."
            );
        }
    }
}
