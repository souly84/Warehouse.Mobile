using System.IO;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Warehouse.Core;
using Warehouse.Mobile.UnitTests.Mocks;
using WebRequest.Elegant.Fakes;
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

        [Fact]
        public async Task SortOperation()
        {
            var items = await new StatefulReception(
                   new EbSoftReception(
                       new WebRequest.Elegant.WebRequest(
                            "http://nonexisting.com",
                            new FkHttpMessageHandler(
                                File.ReadAllText("./Data/UnsortedGoodsData.json")
                            )
                        ),
                       1
                   ).WithExtraConfirmed()
                    .WithoutInitiallyConfirmed(),
                   new KeyValueStorage()
                ).NotConfirmedOnly().ToListAsync();
            items.Sort(new ReceptionGoodComparer());
            Assert.Equal(
                89,
                items.Count
            );
        }
    }
}
