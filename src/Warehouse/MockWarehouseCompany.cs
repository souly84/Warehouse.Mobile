using System;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class MockWarehouseCompany : ICompany
    {
        public MockWarehouseCompany()
            : this(
                  new MockWarehouse(
                    new ListOfEntities<IWarehouseGood>(),
                    new ListOfEntities<IStorage>()
                  )
              )
        {
        }

        public MockWarehouseCompany(IWarehouse warehouse)
        {
            Warehouse = warehouse;
        }

        public IEntities<ICustomer> Customers => throw new NotImplementedException();

        public IEntities<IUser> Users => throw new NotImplementedException();

        public IWarehouse Warehouse { get; }

        public IEntities<ISupplier> Suppliers =>
            new ListOfEntities<ISupplier>(
                new NamedMockSupplier("Electrolux"),
                new NamedMockSupplier("Bosh"),
                new NamedMockSupplier("Samsung")
            )
        ;

        public Task<IUser> LoginAsync(string userName, string password)
        {
            throw new NotSupportedException();
        }

        public void PrintTo(IMedia media)
        {
            // Empty for now
        }
    }
}
