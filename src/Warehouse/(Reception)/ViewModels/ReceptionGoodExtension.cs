﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
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
    }
}
