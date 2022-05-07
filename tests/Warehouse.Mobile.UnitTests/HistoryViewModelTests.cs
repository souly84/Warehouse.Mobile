using System.Threading.Tasks;
using Dotnet.Commands;
using Warehouse.Mobile.ViewModels;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class HistoryViewModelTests
    {
        private App _app = WarehouseMobile
            .Application()
            .GoToReceptionDetails();

        [Fact]
        public async Task SupplierName()
        {
            await _app
                .CurrentViewModel<ReceptionDetailsViewModel>()
                .GoToHistoryCommand.ExecuteAsync();

            Assert.Equal(
                "Electrolux",
                _app.CurrentViewModel<HistoryViewModel>().SupplierName
            );
        }
    }
}
