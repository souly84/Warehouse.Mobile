using System.Collections.Generic;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.UnitTests.Mocks;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class ReceptionExtensions
    {
        public static IReception Stateful(this IReception reception, string state)
        {
            return reception.Stateful(new Dictionary<string, string>
            {
                { $"Repcetion_{reception.Id}", state }
            });
        }

        public static IReception Stateful(this IReception reception, Dictionary<string, string> keyValyeStorage)
        {
            return reception
                .WithExtraConfirmed()
                .WithoutInitiallyConfirmed()
                .Stateful(new KeyValueStorage(keyValyeStorage));
        }
    }
}
