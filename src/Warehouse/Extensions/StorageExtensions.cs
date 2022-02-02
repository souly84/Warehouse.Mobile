using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile.Extensions
{
    public static class StorageExtensions
    {
        public static async Task<int> TotalGoodsQuantityAsync(this IStorage storage)
        {
            var total = 0;
            var goods = await storage.Goods.ToListAsync();
            foreach (var good in goods)
            {
                total += await storage.QuantityForAsync(good);
            }
            return total;
        }
    }
}
