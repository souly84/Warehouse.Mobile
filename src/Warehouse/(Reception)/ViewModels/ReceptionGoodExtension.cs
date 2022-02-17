using System;
using System.Collections.Generic;
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

        public static async Task<IReceptionGood> ByBarcodeAsync(
            this ObservableCollection<ReceptionGroup> receptionGroups,
            string barcodeData,
            bool ignoreConfirmed = false)
        {
            var extraConfrirmed = new List<IReceptionGood>();
            foreach (var receptionGroup in receptionGroups)
            {
                var goods = await receptionGroup.ByBarcodeAsync(barcodeData, ignoreConfirmed);
                if (goods.Any(g => g.IsExtraConfirmed))
                {
                    extraConfrirmed.AddRange(goods);
                }
                else if (goods.Any(g => !g.IsUnknown))
                {
                    return goods.First();
                }
            }

            if (extraConfrirmed.Any())
            {
                return extraConfrirmed.First();
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
