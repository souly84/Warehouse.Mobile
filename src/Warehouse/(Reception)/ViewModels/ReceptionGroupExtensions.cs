using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using Warehouse.Core;

namespace Warehouse.Mobile.ViewModels
{
    public static class ReceptionGroupExtensions
    {
        public static int TotalQuantity(
             this ObservableCollection<ReceptionGroup> receptionGroups)
        {
            return receptionGroups.Sum(
                group => group.Sum((good) => good.Quantity)
            );
        }

        public static int RemainToConfrim(
             this ObservableCollection<ReceptionGroup> receptionGroups)
        {
            return receptionGroups.Sum(
                receptionGroup => receptionGroup.Sum(
                    (x) => x.Regular
                        ? x.Quantity - x.ConfirmedQuantity
                        : 0
                )
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

        public static async Task<bool> TryToConfirmGood(
            this ObservableCollection<ReceptionGroup> receptionGroups,
            IReceptionGood good)
        {
            foreach (var reception in receptionGroups)
            {
                var goodViewModel = reception.FirstOrDefault(x => x.Equals(good));
                if (goodViewModel != null)
                {
                    goodViewModel.IncreaseQuantityCommand.Execute();
                    if (await good.ConfirmedAsync())
                    {
                        reception.Remove(goodViewModel);
                    }
                    return true;
                }
            }
            return false;
        }

        public static async Task<ReceptionGroup> GoodReceptionAsync(
            this ObservableCollection<ReceptionGroup> receptionGroups,
            IReceptionGood receptionGood,
            string barcodeData)
        {
            ReceptionGroup? reception = null;
            if (receptionGood.IsExtraConfirmed)
            {
                reception = await receptionGroups.FirstOrDefaultAsync(async (x) =>
                {
                    var item = await x.ByBarcodeAsync(barcodeData, false);
                    return item.Any(xx => xx.IsExtraConfirmed);
                });
            }
            if (reception == null)
            {
                reception = receptionGroups.First();
            }
            return reception;
        }
    }
}
