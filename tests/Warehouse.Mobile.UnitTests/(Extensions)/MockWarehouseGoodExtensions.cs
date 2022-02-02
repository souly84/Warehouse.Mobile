using Warehouse.Core;

namespace Warehouse.Mobile.UnitTests.Extensions
{
    public static class MockWarehouseGoodExtensions
    {
        public static IWarehouseGood WithPutAway(this MockWarehouseGood good, IStorage storage)
        {
            return good.With(
                new MockStorages(
                    new ListOfEntities<IStorage>(storage),
                    new ListOfEntities<IStorage>(),
                    new ListOfEntities<IStorage>()
                )
            );
        }
    }
}
