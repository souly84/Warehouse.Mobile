using System.Linq;
using EbSoft.Warehouse.SDK;
using Newtonsoft.Json.Linq;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class ReceptionGoodViewModelTests
    {
        private App _app = WarehouseMobile
            .Application(
                new MockWarehouseCompany(
                    new NamedMockSupplier(
                        "Electrolux",
                        new EbSoftReceptionGood(
                            1,
                            JObject.Parse(@"{
                                ""id"": ""38"",
                                ""oa"": ""OA848815"",
                                ""article"": ""MIELE G7100SCICS"",
                                ""qt"": ""2"",
                                ""ean"": ""4002516061731"",
                                ""qtin"": null,
                                ""error_code"": null,
                                ""commentaire"": null,
                                ""itemType"": ""electro""
                            }")
                        )
                    )
                )
            ).GoToReceptionDetails();

        [Fact]
        public void ReceptionGoodName()
        {
            Assert.Equal(
                "MIELE G7100SCICS",
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().Name
            );
        }

        [Fact]
        public void ReceptionGoodOa()
        {
            Assert.Equal(
                "OA848815",
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().Oa
            );
        }

        [Fact]
        public void ReceptionGoodQuantity()
        {
            Assert.Equal(
                2,
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().Quantity
            );
        }

        [Fact]
        public void ReceptionGoodIsMockedReceptionGood()
        {
            Assert.False(
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().IsMockedReceptionGood
            );
        }

        [Fact]
        public void ReceptionGoodConfirmedQuantity()
        {
            Assert.Equal(
                0,
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().ConfirmedQuantity
            );
        }

        [Fact]
        public void ReceptionGoodTotal()
        {
            Assert.Equal(
                0,
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().Total
            );
        }
    }
}
