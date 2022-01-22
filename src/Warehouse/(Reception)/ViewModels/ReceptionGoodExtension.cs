using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class ReceptionGoodExtension
    {
        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(this IEntities<IReceptionGood> receptions)
        {
            var receptionGoods = await receptions.ToListAsync();
            return new ObservableCollection<ReceptionGoodViewModel>(receptionGoods.Select(x => new ReceptionGoodViewModel(x)));
        }

        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(this IConfirmation confirmation)
        {
            var confirmations = await confirmation.ToListAsync();
            return new ObservableCollection<ReceptionGoodViewModel>(confirmations.Select(x => new ReceptionGoodViewModel(x.Good)));
        }
    }
}
