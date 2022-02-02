using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;
using Warehouse.Mobile.ViewModels;
using Warehouse.Mobile.Views;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReceptionDetailsViewTests
    {
        [Fact]
        public void RebindingContext()
        {
            var page = new ReceptionDetailsView();
            var vm1 = new ReceptionDetailsViewModel(
                new MockScanner(),
                new MockPageDialogService(),
                new MockNavigationService()
            );
            vm1.ReceptionGoods = new System.Collections.ObjectModel.ObservableCollection<ReceptionGoodViewModel>();
            page.BindingContext = vm1;
            vm1.ReceptionGoods.Add(new ReceptionGoodViewModel(new MockReceptionGood("1", 1)));
            page.BindingContext = new ReceptionDetailsViewModel(
                new MockScanner(),
                new MockPageDialogService(),
                new MockNavigationService()
            );

            page.BindingContext = null;
            Assert.Null(page.BindingContext);
        }
    }
}
