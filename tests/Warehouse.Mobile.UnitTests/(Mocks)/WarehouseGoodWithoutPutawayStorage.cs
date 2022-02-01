using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class WarehouseGoodWithoutPutawayStorage : IWarehouseGood
    {
        private readonly IWarehouseGood _origin;
        private IStorages? _storages;

        public WarehouseGoodWithoutPutawayStorage(IWarehouseGood origin)
        {
            _origin = origin;
        }

        public int Quantity => _origin.Quantity;

        public IStorages Storages => _storages ?? (_storages = new MockStorages(
            new ListOfEntities<IStorage>(),
            new ListOfEntities<IStorage>(new MockStorage()),
            new ListOfEntities<IStorage>(new MockStorage(this))
        ));

        public IMovement Movement => _origin.Movement;

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj)
                || _origin.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _origin.GetHashCode();
        }

        public void PrintTo(IMedia media)
        {
            _origin.PrintTo(media);
        }
    }
}
