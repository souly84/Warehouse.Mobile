using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class ReceptionDetailsViewModel : BindableBase, IInitializeAsync
    {
        private readonly ICompany _company;

        public ReceptionDetailsViewModel(ICompany company)
        {
            _company = company;
        }

        private IList<ReceptionGoodViewModel> _receptionGoods;
        public IList<ReceptionGoodViewModel> ReceptionGoods
        {
            get => _receptionGoods;
            set => SetProperty(ref _receptionGoods, value);
        }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var sup = await _company.Suppliers.FirstAsync();
            var rec = await sup.Receptions.FirstAsync();
            ReceptionGoods = await rec.Goods.ToViewModelListAsync();
        }
    }
}
