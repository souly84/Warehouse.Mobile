using System;
using System.Collections.Generic;
using Prism.Navigation;
using Warehouse.Core;
using Warehouse.Mobile.UnitTests.Mocks;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class SupplierViewModelTests
    {
        [Theory, MemberData(nameof(SupplierViewModelData))]
        public void ArgumentNullException(ISupplier supplier, INavigationService navigationService)
        {
            Assert.Throws<ArgumentNullException>(
                () => new SupplierViewModel(supplier, navigationService)
            );
        }

        public static IEnumerable<object[]> SupplierViewModelData =>
            new List<object[]>
            {
                new object[] { null, null },
                new object[] { new MockSupplier(), null },
                new object[] { null, new MockNavigationService() }
            };
    }
}
