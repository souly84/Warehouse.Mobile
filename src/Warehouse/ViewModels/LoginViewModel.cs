using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public class LoginViewModel : IInitializeAsync
    {
        private readonly ICompany _company;

        public LoginViewModel(ICompany company)
        {
            _company = company;
        }

        public IList<SupplierViewModel> Suppliers { get; set; }

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            //Suppliers = await _company.Suppliers.ToViewModelListAsync();
        }
    }
}
