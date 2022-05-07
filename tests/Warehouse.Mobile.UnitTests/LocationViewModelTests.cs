using System;
using Warehouse.Core;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class LocationViewModelTests
    {
        [Fact]
        public void ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new LocationViewModel(null)
            );
        }

        [Fact]
        public void Location()
        {
            Assert.Equal(
                "1234567889",
                new LocationViewModel(new MockStorage()).Location
            );
        }
    }
}
