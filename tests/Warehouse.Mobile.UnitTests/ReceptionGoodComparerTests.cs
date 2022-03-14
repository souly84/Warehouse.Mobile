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
