using System.Collections.Generic;
using Dotnet.Commands;
using Warehouse.Core;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReceptionGroupTests
    {
        [Fact]
        public void NameContainsReceptionId()
        {
            Assert.Equal(
                "Reception 1211",
                new ReceptionGroup(
                    new MockReception("1211"),
                    new List<ReceptionGoodViewModel>()
                ).Name
            );
        }

        [Fact]
        public void TotalCount()
        {
            Assert.Equal(
                "2/2",
                new ReceptionGroup(
                    new MockReception("1211"),
                    new ReceptionGoodViewModel(new MockReceptionGood("1", 1)),
                    new ReceptionGoodViewModel(new MockReceptionGood("2", 3))
                ).TotalCount
            );
        }
    }
}
