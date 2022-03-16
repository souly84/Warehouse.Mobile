using System;
using System.Collections.Generic;
using Dotnet.Commands;
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
        public void ArgumentNullException(
            ISupplier supplier,
            ICommands commands,
            IOverlay overlay,
            INavigationService navigationService)
        {
            Assert.Throws<ArgumentNullException>(
                () => new SupplierViewModel(supplier, commands, overlay, navigationService)
            );
        }

        public static IEnumerable<object[]> SupplierViewModelData =>
            new List<object[]>
            {
                new object[] { null, null, null, null },
                new object[] { new MockSupplier(), null, new MockOverlay(), new MockNavigationService() },
                new object[] { new MockSupplier(), new Commands(), null,  new MockNavigationService() },
                new object[] { new MockSupplier(), new Commands(), new MockOverlay(), null },
            };
    }
}
