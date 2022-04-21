using System;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    [Obsolete("Should be moved to Warehouse.Core")]
    public class MockWarehouseCompany : ICompany
    {
        public MockWarehouseCompany()
            : this(
                  new MockWarehouse(
                    new MockStorage(
                          "ST01"
                    )
                  )
              )
        {
        }

        public MockWarehouseCompany(params IWarehouseGood[] goods)
            : this(
                  new MockWarehouse(
                      new MockStorage(
                          "ST01",
                          goods
                    )
                  )
              )
        {
        }

        public MockWarehouseCompany(params IStorage[] storages)
            : this(new MockWarehouse(storages))
        {
        }

        public MockWarehouseCompany(IWarehouse warehouse)
            : this(
                  warehouse,
                  new ListOfEntities<ISupplier>(
                    new MockSupplier("Electrolux"),
                    new MockSupplier("Bosh"),
                    new MockSupplier("Samsung")
                  )
              )
        {
        }

        public MockWarehouseCompany(params ISupplier[] suppliers)
            : this(
                   new ListOfEntities<ISupplier>(suppliers)
              )
        {
        }

        public MockWarehouseCompany(IEntities<ISupplier> suppliers)
            : this(
                  new MockWarehouse(
                    new MockStorage(
                          "ST01"
                    )
                  ),
                  suppliers
              )
        {
        }

        public MockWarehouseCompany(IWarehouse warehouse, IEntities<ISupplier> suppliers)
        {
            Warehouse = warehouse;
            Suppliers = suppliers;
        }

        public IEntities<ICustomer> Customers => throw new NotImplementedException();

        public IEntities<IUser> Users => throw new NotImplementedException();

        public IWarehouse Warehouse { get; }

        public IEntities<ISupplier> Suppliers { get; }

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
