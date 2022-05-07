using System.Threading.Tasks;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public static class GoodsExtensions
    {
        public static Task<IWarehouseGood> FirstAsync(this IEntities<IWarehouseGood> entities, string barcode)
        {
             return entities.For(barcode).FirstAsync();
        }
    }
}
