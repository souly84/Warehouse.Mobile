using System;
using System.Threading.Tasks;
using Android.Content;
using AndroidHUD;
using Warehouse.Mobile;
using Xamarin.Forms;

namespace Warehouse.Droid.Services
{
    public class AndHudOverlay: IOverlay
    {
        private readonly Context _context;

        public AndHudOverlay(Context context)
        {
            _context = context;
        }

        public async Task OverlayAsync(Func<Task> progressAction, string message)
        {
            _ = progressAction ?? throw new ArgumentNullException(nameof(progressAction));
            var wasClosed = false;
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!wasClosed)
                    {
                        AndHUD.Shared.Show(_context, message);
                    }
                });
                await progressAction().ConfigureAwait(false);
            }
            finally
            {
                wasClosed = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    AndHUD.Shared.Dismiss(_context);
                });
            }
        }
    }
}
