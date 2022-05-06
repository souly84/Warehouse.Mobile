using System.Threading.Tasks;
using Dotnet.Commands;
using Warehouse.Core;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    public class ReceptionGoodExtensionTests
    {
        [Fact]
        public async Task ToViewModels()
        {
            Assert.Equal(
                2,
                (await new ListOfEntities<IReceptionGood>(
                    new MockReceptionGood("1", 2),
                    new MockReceptionGood("2", 3)
                ).ToViewModelListAsync(new Commands())).Count
            );
        }
    }
}
