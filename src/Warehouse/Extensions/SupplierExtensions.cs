using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dotnet.Commands;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

namespace Warehouse.Mobile.Extensions
{
    public static class SupplierExtensions
    {
        public static async Task<ObservableCollection<ReceptionGroup>> ReceptionViewModelsAsync(
            this ISupplier supplier,
            ICommands commands,
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
                            .ToViewModelListAsync(commands)
                    )
                );
            }
            return receptionGroups;
        }
    }
}
