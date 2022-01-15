using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class NamedMockSupplier : ISupplier
    {
        private readonly string _supplierName;
        private readonly ISupplier _supplier;

        public NamedMockSupplier()
            : this("MockSupplier")
        {
        }

        public NamedMockSupplier(string supplierName) : this(
            supplierName,
            new MockSupplier()
        )
        {
        }

        public NamedMockSupplier(string supplierName, params IReceptionGood[] goods)
            : this(supplierName, new MockReception(goods))
        {
        }

        public NamedMockSupplier(string supplierName, params IReception[] receptions)
           : this(supplierName, new MockSupplier(receptions))
        {
        }

        public NamedMockSupplier(string supplierName, ISupplier supplier)
        {
            _supplierName = supplierName;
            _supplier = supplier;
        }

        public IEntities<IReception> Receptions => _supplier.Receptions;

        public void PrintTo(IMedia media)
        {
            _supplier.PrintTo(media);
            media.Put("name", _supplierName);
        }
    }
}
