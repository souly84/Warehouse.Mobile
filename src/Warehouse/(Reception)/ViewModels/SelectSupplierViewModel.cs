using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class SelectSupplierViewModel : BindableBase, IInitializeAsync
    {
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialog;

        public SelectSupplierViewModel(
            ICompany company,
            INavigationService navigationService,
            IPageDialogService dialog)
        {
            _company = company ?? throw new ArgumentNullException(nameof(company));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _dialog = dialog;
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

        public Task InitializeAsync(INavigationParameters parameters)
        {
            SelectedDate = DateTime.Now;
            return Task.CompletedTask;
            //return RefreshAvailableSupplierList();
        }

        private DelegateCommand? changeSelectedDateCommand;
        public DelegateCommand ChangeSelectedDateCommand => changeSelectedDateCommand ?? (changeSelectedDateCommand = new DelegateCommand(async () =>
        {
            await RefreshAvailableSupplierList();
        }));

        private async Task RefreshAvailableSupplierList()
        {
            try
            {
                Suppliers = await _company
                .Suppliers
                .For(SelectedDate)
                .ToViewModelListAsync(_navigationService);
            }
            catch (InvalidOperationException ex)
            {
                await _dialog.DisplayAlertAsync("Error", "Please check your connection", "Ok");
            }
            catch (Exception ex)
            {
                await _dialog.DisplayAlertAsync("Error", ex.Message.ToString(), "Ok");
            }

        }
    }
}
