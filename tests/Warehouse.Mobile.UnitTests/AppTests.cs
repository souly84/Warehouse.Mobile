﻿using Warehouse.Mobile.UnitTests.Extensions;
using Xunit;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class AppTests
    {
        private App _app = XamarinFormsTests.InitPrismApplication();

        [Fact]
        public void ScannerInitialized()
        {
            Assert.NotNull(
               _app.Scanner
            );
        }

        [Fact]
        public void NavigationInitialized()
        {
            Assert.NotNull(
                _app.Navigation
            );
        }

        [Fact]
        public void MenuSelectionView_AsDefaultViewOnApplicationStartup()
        {
            Assert.Equal(
                "/NavigationPage/MenuSelectionView",
                _app.GetNavigationUriPath()
            );
        }
    }
}
