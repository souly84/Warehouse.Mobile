using System.Collections.Generic;
using Warehouse.Core;
using Warehouse.Mobile.Extensions;
using Warehouse.Mobile.UnitTests.Mocks;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class ReceptionExtensions
    {
        public static IReception WithConfirmationProgress(this IReception reception, string state)
        {
            return reception.WithConfirmationProgress(new Dictionary<string, string>
            {
                { $"Repcetion_{reception.Id}", state }
            });
        }

        public static IReception WithConfirmationProgress(this IReception reception, Dictionary<string, string> keyValyeStorage)
        {
            return reception
                .WithExtraConfirmed()
                .WithoutInitiallyConfirmed()
                .WithConfirmationProgress(new KeyValueStorage(keyValyeStorage));
        }
    }
}
