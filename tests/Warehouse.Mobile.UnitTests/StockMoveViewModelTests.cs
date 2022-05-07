using System;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Services;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.ViewModels;
using Xunit;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class StockMoveViewModelTests
    {
        private App _app = WarehouseMobile.Application(
            new MockWarehouseCompany(
               new MockStorage(
                   "ST01",
                   new MockWarehouseGood("1", 1, "1111")
               ),
               new MockStorage(
                   "ST02",
                   new MockWarehouseGood("1", 1, "1111")
               )
            )
        ).GoToStockMovement();

        [Fact]
        public void StorageGoodMovement_ShowsQuantityPopup()
        {
            _app
                .Scan(new ScanningResult("ST01", "code128", DateTime.Now.TimeOfDay))
                .Scan("1111")
                .Scan(new ScanningResult("ST02", "code128", DateTime.Now.TimeOfDay));
            
            Assert.IsType<QuantityToMovePopupViewModel>(
                _app.CurrentViewModel<object>()
            );
        }

        [Fact]
        public void ScannedProductToMove()
        {
            _app
                .Scan(new ScanningResult("ST01", "code128", DateTime.Now.TimeOfDay))
                .Scan("1111");

            Assert.True(
                _app.CurrentViewModel<StockMoveViewModel>().IsRecognizedProduct
            );
            Assert.Equal(
                "1111",
                _app.CurrentViewModel<StockMoveViewModel>().ScannedProductToMove
            );
        }

        [Fact]
        public void OriginLocation()
        {
            _app
                .Scan(new ScanningResult("ST01", "code128", DateTime.Now.TimeOfDay))
                .Scan("1111");

            Assert.True(
               _app.CurrentViewModel<StockMoveViewModel>().IsRecognizedOriginLocation
           );
            Assert.NotNull(
                _app.CurrentViewModel<StockMoveViewModel>().OriginLocationVm
            );
            Assert.Equal(
                "ST01",
                _app.CurrentViewModel<StockMoveViewModel>().OriginLocationVm.Location
            );
        }

        [Fact]
        public void RaceLocations()
        {
            _app
                .Scan(new ScanningResult("ST01", "code128", DateTime.Now.TimeOfDay))
                .Scan("1111");

            Assert.NotEmpty(
               _app.CurrentViewModel<StockMoveViewModel>().RaceLocations
            );
        }

        [Fact]
        public void ReserveLocations()
        {
            _app
                .Scan(new ScanningResult("ST01", "code128", DateTime.Now.TimeOfDay))
                .Scan("1111");

            Assert.NotEmpty(
               _app.CurrentViewModel<StockMoveViewModel>().ReserveLocations
            );
        }

        [Fact]
        public void ErrorAlert_WhenNoOriginLocationIsSpecified()
        {
            _app.Scan("1111");
            Assert.Contains(
                new DialogPage
                {
                    Title = "Error",
                    Message = "Please scan the location you wants to move from before scanning a product",
                    CancelButton = "Ok",
                },
                (_app.Resolve<IPageDialogService>() as MockPageDialogService).ShownDialogs
            );
        }

        [Fact]
        public async Task BackCommand_GoesBackToMenuSelectionView()
        {
            await _app
                .CurrentViewModel<StockMoveViewModel>()
                .BackCommand.ExecuteAsync();

            Assert.IsType<MenuSelectionViewModel>(
                _app.CurrentViewModel<object>()
            );
        }
    }
}
