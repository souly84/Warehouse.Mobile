using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class WarehouseGoodExtensions
    {
        public static async Task<ObservableCollection<LocationViewModel>> ToViewModelListAsync(this IEntities<IStorage> storages)
        {
            var storagesList = await storages.ToListAsync();
            return new ObservableCollection<LocationViewModel>(
                storagesList.Select(x => new LocationViewModel(x))
            );
        }
    }
}