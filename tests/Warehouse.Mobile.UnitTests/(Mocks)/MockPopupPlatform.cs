using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Warehouse.Mobile.UnitTests.Extensions;
using Warehouse.Mobile.UnitTests.Mocks;
using Xamarin.Forms;
using static Warehouse.Mobile.Tests.MockPageDialogService;

[assembly: Dependency(typeof(MockPopupPlatform))]

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class MockPopupPlatform : IPopupPlatform
    {
        public bool IsInitialized => true;

        public bool IsSystemAnimationEnabled => false;

        public event EventHandler OnInitialized;

        public List<DialogPage> ShownPopups { get; private set; } = new List<DialogPage>();

        public List<DialogPage> VisiblePopup { get; private set; } = new List<DialogPage>();

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;
            ShownPopups.Add(page.ToDialogPage());
            VisiblePopup.Add(page.ToDialogPage());
            return Task.CompletedTask;
        }

        public Task RemoveAsync(PopupPage page)
        {
            VisiblePopup.Remove(page.ToDialogPage());
            return Task.CompletedTask;
        }

        public void RaiseOnInitialized()
        {
            OnInitialized?.Invoke(this, EventArgs.Empty);
        }
    }
}
