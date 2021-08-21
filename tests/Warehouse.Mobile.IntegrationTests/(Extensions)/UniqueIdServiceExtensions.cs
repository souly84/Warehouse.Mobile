using System;
using System.Threading.Tasks;

namespace StoreMobile.AppService.Services
{
    public static class UniqueIdServiceExtensions
    {
        public static async Task AssignNewDevicesAsync(this IUniqueIdService uniqueIdService, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                await uniqueIdService.AssignNewAsync(Guid.NewGuid().ToString());
            }
        }
    }
}
