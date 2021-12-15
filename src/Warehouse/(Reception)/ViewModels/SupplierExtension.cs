using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class SupplierExtension
    {
        public static async Task<IList<SupplierViewModel>> ToViewModelListAsync(this IEntities<ISupplier> suppliers)
        {
            var supplierList = await suppliers.ToListAsync();
            return new ObservableCollection<SupplierViewModel>(supplierList.Select(x => new SupplierViewModel(x)));
        }
    }
}
