using System.Threading.Tasks;
using EbSoft.Warehouse.SDK;
using Warehouse.Core;
using WebRequest.Elegant.Fakes;
using Xunit;


namespace Warehouse.Mobile.UnitTests
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(
                "Hello world",
                "Hello world"
            );
        }

        [Fact]
        public async Task MoveFromPutAwayToRace()
        {
            var proxy = new ProxyHttpMessageHandler();
            var company = new EbSoftCompany(
                new WebRequest.Elegant.WebRequest(
                    AppConstants.Uri, proxy)
            );
            var good = await company
                .Warehouse
                .Goods.For("4242005322251")
                .FirstAsync();

            var reserve = await good.Storages.Reserve.ToListAsync();

            await good.Movement
                .From(await good.Storages.Reserve.FirstAsync())
                .MoveToAsync(
                    await good.Storages.ByBarcodeAsync(company.Warehouse, "134029475445"),
                    1
            );

            Assert.EqualJson(
                    @"{""error"":""0"",""message"":""Transfert ok.""}",
                 proxy.ResponsesContent[1]
            );
        }
    }
}
