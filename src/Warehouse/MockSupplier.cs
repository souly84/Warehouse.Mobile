using System;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class MockSupplier : ISupplier
    {
        private readonly string _supplierName;

        public MockSupplier()
            : this("MockSupplier")
        {
        }

        public MockSupplier(string supplierName) : this(
            supplierName,
            new MockReception(
                new MockReceptionGood("1", 2, "123456789"),
                new MockReceptionGood("2", 2, "123456780"),
                new MockReceptionGood("3", 2, "123456781"),
                new MockReceptionGood("4", 2, "123456782")
            )
        )
        {
        }

        public MockSupplier(string supplierName, params IReception[] receptions)
            : this(supplierName, new ListOfEntities<IReception>(receptions))
        {
        }

        public MockSupplier(string supplierName, IEntities<IReception> receptions)
        {
            _supplierName = supplierName;
            Receptions = receptions;
        }

        public IEntities<IReception> Receptions { get; }

        public void PrintTo(IMedia media)
        {
            media
                .Put("name", _supplierName)
                .Put("receptions", Receptions);
        }
    }
}
