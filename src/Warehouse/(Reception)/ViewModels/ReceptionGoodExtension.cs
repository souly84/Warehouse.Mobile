using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Mobile.Reception.ViewModels;
using Warehouse.Mobile.Reception.Views;

namespace Warehouse.Mobile.ViewModels
{
    public static class ReceptionGoodExtension
    {
        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(this IEntities<IReceptionGood> receptions)
        {
            return new ObservableCollection<ReceptionGoodViewModel>(
                await receptions.SelectAsync(x => new ReceptionGoodViewModel(x))
            );
        }

        public static async Task<ObservableCollection<ReceptionGoodViewModel>> ToViewModelListAsync(this IConfirmation confirmation)
        {
            var confirmations = await confirmation.ToListAsync();
            confirmations.Sort(new ReceptionGoodComparer());
            return new ObservableCollection<ReceptionGoodViewModel>(confirmations
            .Select(x => new ReceptionGoodViewModel(x.Good)));
        }

        public static async Task<IReceptionGood> ByBarcodeAsync(
            this ObservableCollection<ReceptionGroup> receptionGroups,
            string barcodeData,
            bool ignoreConfirmed = false)
        {
            foreach (var receptionGroup in receptionGroups)
            {
                var goods = await receptionGroup.ByBarcodeAsync(barcodeData, ignoreConfirmed);
                if (goods.Any(g => !g.IsUnknown))
                {
                    return goods.First();
                }
            }

            if (receptionGroups.Any())
            {
                var goods = await receptionGroups.First().ByBarcodeAsync(barcodeData);
                return goods.First();
            }

            throw new InvalidOperationException(
                $"Reception groups dont contain any good with barcode {barcodeData}"
            );
        }
    }
}
