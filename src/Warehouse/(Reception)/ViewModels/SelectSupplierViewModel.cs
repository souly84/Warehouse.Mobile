using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class SelectSupplierViewModel : BindableBase, IInitializeAsync
    {
        private readonly ICompany _company;
        private readonly INavigationService _navigationService;

        public SelectSupplierViewModel(
            ICompany company,
            INavigationService navigationService)
        {
            _company = company;
            _navigationService = navigationService;
        }

        private IList<SupplierViewModel> _suppliers;
        public IList<SupplierViewModel> Suppliers
        {
            get => _suppliers;
            set => SetProperty(ref _suppliers, value);
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            CurrentDate = new DateTime(2021, 12, 16);
            Suppliers = await _company.Suppliers.For(CurrentDate).ToViewModelListAsync(_navigationService);
        }
    }
}
