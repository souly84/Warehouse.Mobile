using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class QuantityToMovePopupViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .QuantityToMovePopup();

        [Fact]
        public void QuantityToMovePopupViewModelOnTheTopOfTheStack()
        {
            Assert.IsType<QuantityToMovePopupViewModel>(_app.CurrentViewModel<object>());
        }
    }
}
