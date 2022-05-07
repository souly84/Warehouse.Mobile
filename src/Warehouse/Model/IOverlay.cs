using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warehouse.Mobile
{
    public interface IOverlay
    {
        Task OverlayAsync(Func<Task> progressAction, string message);
    }

    public class MockOverlay : IOverlay
    {
        public bool IsVisible { get; private set; }

        public string VisibleMessage { get; private set; } = string.Empty;

        public List<string> ShownMessages { get; private set; } = new List<string>();

        public async Task OverlayAsync(Func<Task> progressAction, string message)
        {
            _ = progressAction ?? throw new ArgumentNullException(nameof(progressAction));
            try
            {
                IsVisible = true;
                VisibleMessage = message;
                ShownMessages.Add(message);
                await progressAction().ConfigureAwait(false);
            }
            finally
            {
                IsVisible = false;
                VisibleMessage = string.Empty;
            }
        }
    }
}
