using System;
using System.Linq;
using EbSoft.Warehouse.SDK;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
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
                    new MockSupplier(
                        "Electrolux",
                        new MockReception(
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
                    )
                )
            ).GoToReceptionDetails();

        [Fact]
        public void ThrowArgumentNullReferenceException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ReceptionGoodViewModel(null)
            );
        }

        [Fact]
        public void ExtraConfirmedGood()
        {
            Assert.True(
                new ReceptionGoodViewModel(
                    new ExtraConfirmedReceptionGood(
                        new MockReceptionGood("1", 1)
                    )
                ).IsExtraConfirmedReceptionGood
            );
        }

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
        public void ReceptionGoodRemainingQuantity()
        {
            Assert.Equal(
                "0/2",
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().RemainingQuantity
            );
        }

        [Fact]
        public void UnknownReceptionGoodRemainingQuantity()
        {
            Assert.Equal(
                "0",
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .ElementAt(1)
                    .RemainingQuantity
            );
        }

        [Fact]
        public void ReceptionGoodIsMockedReceptionGood()
        {
            Assert.False(
                _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First().IsUnkownGood
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

        [Fact]
        public void IncreaseQuantityCommandIncreasesConfirmedQuantity()
        {
            var goodViewModel = _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First();
            goodViewModel.IncreaseQuantityCommand.Execute();
            Assert.Equal(
                1,
                goodViewModel.ConfirmedQuantity
            );
        }

        [Fact]
        public void DecreaseQuantityCommandDecreasesConfirmedQuantity()
        {
            var goodViewModel = _app
                    .CurrentViewModel<ReceptionDetailsViewModel>()
                    .ReceptionGoods
                    .First();
            goodViewModel.IncreaseQuantityCommand.Execute();
            goodViewModel.DecreaseQuantityCommand.Execute();
            Assert.Equal(
                0,
                goodViewModel.ConfirmedQuantity
            );
        }
    }
}
