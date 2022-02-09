using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Reception.ViewModels;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile.Extensions
{
    public static class SupplierExtensions
    {
        public static async Task<ObservableCollection<ReceptionGroup>> ReceptionViewModelsAsync(
            this ISupplier supplier,
            IKeyValueStorage keyValueStorage)
        {
            var receptions = await supplier.Receptions.ToListAsync();
            var receptionGroups = new ObservableCollection<ReceptionGroup>();
            foreach (var reception in receptions)
            {
                var statefulReception = reception
                   .WithExtraConfirmed()
                   .WithoutInitiallyConfirmed()
                   .WithConfirmationProgress(keyValueStorage);
                receptionGroups.Add(
                    new ReceptionGroup(
                        statefulReception,
                        await statefulReception
                            .NotConfirmedOnly()
                            .ToViewModelListAsync()
                    )
                );
            }
            return receptionGroups;
        }
    }
}
