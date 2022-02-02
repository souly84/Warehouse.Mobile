using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class WarehouseGoodExtensions
    {
        public static async Task<ObservableCollection<LocationViewModel>> ToViewModelListAsync(this IEntities<IStorage> storages)
        {
            return new ObservableCollection<LocationViewModel>(
                await storages.SelectAsync(x => new LocationViewModel(x))
            );
        }
    }
}