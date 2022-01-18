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

        public MockWarehouseCompany(params IWarehouseGood[] goods)
            : this(
                  new MockWarehouse(
                      new ListOfEntities<IWarehouseGood>(goods),
                      new ListOfEntities<IStorage>(
                          new MockStorage(
                              "ST01",
                              goods
                          )
                      )
                  )
              )
        {
        }

        public MockWarehouseCompany(IWarehouse warehouse)
            : this(
                  warehouse,
                  new ListOfEntities<ISupplier>(
                    new NamedMockSupplier("Electrolux"),
                    new NamedMockSupplier("Bosh"),
                    new NamedMockSupplier("Samsung")
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
                    new ListOfEntities<IWarehouseGood>(),
                    new ListOfEntities<IStorage>()
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
