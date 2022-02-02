using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Navigation;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class SupplierExtension
    {
        public static async Task<IList<SupplierViewModel>> ToViewModelListAsync(this IEntities<ISupplier> suppliers, INavigationService navigationService)
        {
            return new ObservableCollection<SupplierViewModel>(
                await suppliers.SelectAsync(x => new SupplierViewModel(x, navigationService))
            );
        }
    }
}
