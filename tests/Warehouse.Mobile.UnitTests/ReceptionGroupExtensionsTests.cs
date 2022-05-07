using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReceptionGroupExtensionsTests
    {
        [Fact]
        public Task InvalidOperationException_WhenNoBarcodeGoodFound()
        {
            return Assert.ThrowsAsync<InvalidOperationException>(
                () => new ObservableCollection<ReceptionGroup>().ByBarcodeAsync("aaaa")
            );
        }
    }
}
