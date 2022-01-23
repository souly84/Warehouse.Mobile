using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class QuantityToMovePopupViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .QuantityToMovePopup();

        public QuantityToMovePopupViewModelTests()
        {
        }
    }
}
