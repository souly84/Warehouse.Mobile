using Warehouse.Core;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.Extensions
{
    public static class ReceptionExtensions
    {
        public static IReception Stateful(this IReception reception, IKeyValueStorage keyValueStorage)
        {
            return new StatefulReception(reception, keyValueStorage);
        }
    }
}
