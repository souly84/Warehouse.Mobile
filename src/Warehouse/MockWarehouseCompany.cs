using System;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;

namespace Warehouse.Mobile
{
    public class MockWarehouseCompany : ICompany
    {
        public IEntities<ICustomer> Customers => throw new NotImplementedException();

        public IEntities<IUser> Users => throw new NotImplementedException();

        public IWarehouse Warehouse => throw new NotImplementedException();

        public IEntities<ISupplier> Suppliers =>
            new ListOfEntities<ISupplier>(
                new MockSupplier("Electrolux"),
                new MockSupplier("Bosh"),
                new MockSupplier("Samsung")
            )
        ;

        public Task<IUser> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void PrintTo(IMedia media)
        {
        }
    }
}
