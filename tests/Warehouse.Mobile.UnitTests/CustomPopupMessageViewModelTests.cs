using System;
using Warehouse.Mobile.ViewModels;
using Warehouse.Mobile.Extensions;
using Xunit;
using System.Threading.Tasks;
using Dotnet.Commands;

namespace Warehouse.Mobile.UnitTests
{
    [Collection(XUnitCollectionDefinitions.NavigationDependent)]
    public class CustomPopupMessageViewModelTests
    {
        private App _app = WarehouseMobile.Application();

        [Fact]
        public void ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CustomPopupMessageViewModel(null, null)
            );
        }

        [Fact]
        public void Severity()
        {
            try
            {
                _ = _app.PageNavigationService().ShowMessageAsync(
                    PopupSeverity.Warning,
                    "Test Title",
                    "Test Message"
                );
                Assert.Equal(
                    PopupSeverity.Warning,
                    _app.CurrentViewModel<CustomPopupMessageViewModel>().Severity
                );
            }
            finally
            {
                _app.ClosePopup();
            }
        }

        [Fact]
        public void Title()
        {
            try
            {
                _ = _app.PageNavigationService().ShowMessageAsync(
                    PopupSeverity.Warning,
                    "Test Title",
                    "Test Message"
                );
                Assert.Equal(
                    "Test Title",
                    _app.CurrentViewModel<CustomPopupMessageViewModel>().Title
                );
            }
            finally
            {
                _app.ClosePopup();
            }
        }

        [Fact]
        public void Message()
        {
            try
            {
                _ = _app.PageNavigationService().ShowMessageAsync(
                    PopupSeverity.Warning,
                    "Test Title",
                    "Test Message"
                );
                Assert.Equal(
                    "Test Message",
                    _app.CurrentViewModel<CustomPopupMessageViewModel>().Message
                );
            }
            finally
            {
                _app.ClosePopup();
            }
        }

        [Fact]
        public async Task Callback()
        {
            try
            {
                var callbackTask = _app.PageNavigationService().ShowMessageAsync(
                    PopupSeverity.Warning,
                    "Test Title",
                    "Test Message"
                );
                _app.CurrentViewModel<CustomPopupMessageViewModel>().ActionCommand.Execute();
                await callbackTask;
                Assert.IsType<MenuSelectionViewModel>(_app.CurrentViewModel<object>());
            }
            finally
            {
                _app.ClosePopup();
            }
        }

        [Fact]
        public async Task Callback_ThrowsException()
        {
            try
            {
                await Assert.ThrowsAsync<InvalidOperationException>(
                    () => _app.Navigation.ShowMessageAsync(
                        PopupSeverity.Warning,
                        "Test Title",
                        "Test Message"
                    )
                );
            }
            finally
            {
                _app.ClosePopup();
            }
        }
    }
}
