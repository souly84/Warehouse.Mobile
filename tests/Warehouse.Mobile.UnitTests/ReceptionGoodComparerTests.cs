using Warehouse.Core;
using Warehouse.Mobile.Reception.Views;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReceptionGoodComparerTests
    {
        [Fact]
        public void NullEqualToNull()
        {
            Assert.Equal(
                0,
                new ReceptionGoodComparer().Compare(null, null)
            );
        }

        [Fact]
        public void NotNullIsGreaterThanNull()
        {
            Assert.Equal(
                1,
                new ReceptionGoodComparer().Compare(
                    new GoodConfirmation(new MockReceptionGood("1", 1), 1),
                    null
                )
            );
        }

        [Fact]
        public void NullIsLessThanNonNull()
        {
            Assert.Equal(
                -1,
                new ReceptionGoodComparer().Compare(
                    null,
                    new GoodConfirmation(new MockReceptionGood("1", 1), 1)
                )
            );
        }
    }
}
