using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Navigation;
using Prism.Services;
using Warehouse.Mobile.Interfaces;
using Warehouse.Mobile.Tests;
using Warehouse.Mobile.UnitTests.Mocks;
using Warehouse.Mobile.ViewModels;
using Xunit;
using static Warehouse.Mobile.Tests.MockPageDialogService;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class MenuSelectionViewModelTests
    {
        private App _app = WarehouseMobile.Application();

        [Theory, MemberData(nameof(MenuSelectionViewModelData))]
        public void ArgumentNullException(
            IPageDialogService dialog,
            INavigationService navigationService,
            ICommands commands,
            IEnvironment environment)
        {
            Assert.Throws<ArgumentNullException>(
                () => new MenuSelectionViewModel(
                    dialog,
                    navigationService,
                    commands,
                    environment
                )
            );
        }

        [Fact]
        public void SuppliersNavigation()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/SelectSupplierView",
                 _app.GoToSuppliers().GetNavigationUriPath()
            );

            Assert.IsType<SelectSupplierViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public void PutAwayNavigation()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/PutAwayView",
                _app.GoToPutAway().GetNavigationUriPath()
            );

            Assert.IsType<PutAwayViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public void StockMovementNavigation()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView/StockMoveView",
                 _app.GoToStockMovement().GetNavigationUriPath()
            );

            Assert.IsType<StockMoveViewModel>(_app.CurrentViewModel<object>());
        }

        [Fact]
        public async Task GoBack_ShowsWarningDialog()
        {
            var dialog = new MockPageDialogService();
            await WarehouseMobile.Application(
                new MockPlatformInitializer(
                    pageDialogService: dialog
                )
            ).CurrentViewModel<MenuSelectionViewModel>()
             .BackCommand.ExecuteAsync();
            Assert.Contains(
                new DialogPage
                {
                    Title = "Warning",
                    Message = "Are you sure you want to quit the application?",
                    AcceptButton = "Yes",
                    CancelButton = "No"
                },
                dialog.ShownDialogs
            );
        }

        public static IEnumerable<object[]> MenuSelectionViewModelData =>
          new List<object[]>
          {
                new object[] { null, null, null, null },
                new object[] { new MockPageDialogService(), null, new Commands(), new MockEnvironment() },
                new object[] { new MockPageDialogService(), new MockNavigationService(), null, new MockEnvironment() },
                new object[] { new MockPageDialogService(), new MockNavigationService(), new Commands(), null },
          };
    }
}
