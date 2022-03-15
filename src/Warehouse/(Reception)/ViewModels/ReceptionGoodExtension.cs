using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class ReceptionGoodExtension
    {
        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(
            this IEntities<IReceptionGood> receptions,
            ICommands commands)
        {
            return new ObservableCollection<ReceptionGoodViewModel>(
                await receptions.SelectAsync(x => new ReceptionGoodViewModel(x, commands))
            );
        }

        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(
            this IConfirmation confirmation,
            ICommands commands)
        {
            var confirmations = await confirmation.ToListAsync();
            confirmations.Sort(new ReceptionGoodComparer());
            return new ObservableCollection<ReceptionGoodViewModel>(
                confirmations
                    .Select(x => new ReceptionGoodViewModel(x.Good, commands))
            );
        }
    }
}
