using System.Threading.Tasks;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class AsyncDataExtensions
    {
        public static async Task<T> ValueAsync<T>(this AsyncData<T> data)
        {
            await data.WaitWhileLoading();
            return data.Value;
        }
    }
}
