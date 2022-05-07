using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class SupplierExtension
    {
        public static async Task<IList<SupplierViewModel>> ToViewModelListAsync(
            this IEntities<ISupplier> suppliers,
            INavigationService navigationService,
            IOverlay overlay,
            ICommands commands)
        {
            return new ObservableCollection<SupplierViewModel>(
                (await suppliers.SelectAsync(supplier =>
                    new SupplierViewModel(
                        supplier,
                        commands,
                        overlay,
                        navigationService
                    )
                )).ToList()
            );
        }
    }
}
