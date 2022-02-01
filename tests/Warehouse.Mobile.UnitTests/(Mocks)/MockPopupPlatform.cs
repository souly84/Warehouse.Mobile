using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Warehouse.Mobile.UnitTests.Mocks;
using Xamarin.Forms;

[assembly: Dependency(typeof(MockPopupPlatform))]

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class MockPopupPlatform : IPopupPlatform
    {
        public bool IsInitialized => true;

        public bool IsSystemAnimationEnabled => false;

        public event EventHandler OnInitialized;

        public List<PopupPage> ShownPopups { get; private set; } = new List<PopupPage>();

        public List<PopupPage> VisiblePopup { get; private set; } = new List<PopupPage>();

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;
            ShownPopups.Add(page);
            VisiblePopup.Add(page);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(PopupPage page)
        {
            VisiblePopup.Remove(page);
            return Task.CompletedTask;
        }
    }
}
