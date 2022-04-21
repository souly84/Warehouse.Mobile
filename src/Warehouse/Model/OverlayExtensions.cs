using System;
using System.Threading.Tasks;

namespace Warehouse.Mobile
{
    public static class OverlayExtensions
    {
        public static async Task<T?> OverlayAsync<T>(
            this IOverlay overlay,
            Func<Task<T>> progressAction,
            string message)
        {
            T? result = default;
            await overlay.OverlayAsync(async () =>
            {
                result = await progressAction();
            }, message);
            return result;
        }
    }
}
