using Xunit;

namespace Warehouse.Mobile.IntegrationTests
{
    [Collection("Sequential")]
    public class IntegrationTests
    {
        // skip constant should be not null in case the whole test scenario need to be skipped
        private const string skip = null; // "Tests StoreTests skipped";

        [Fact(Skip = skip)]
        public void Test1()
        {
            Assert.True(
                true
            );
        }
    }
}
