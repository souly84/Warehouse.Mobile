using System.IO;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Newtonsoft.Json.Linq;
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

        [Fact]
        public void Compare_WithUnknownGood()
        {
            Assert.Equal(
                1,
                new ReceptionGoodComparer().Compare(
                     new EbSoftReceptionGood(
                         1,
                         JObject.Parse(@"{
                             ""id"": ""38"",
                             ""oa"": ""OA848815"",
                             ""article"": ""MIELE G7100SCICS"",
                             ""qt"": ""2"",
                             ""ean"": [ ""4002516061731"" ],
                             ""qtin"": 0,
                             ""error_code"": null,
                             ""commentaire"": null,
                             ""itemType"": ""electro""
                         }")
                     ),
                     new EbSoftReceptionGood(
                         1,
                         "SomeUnexpectedBarcode"
                     )
                )
            );
        }
    }
}
