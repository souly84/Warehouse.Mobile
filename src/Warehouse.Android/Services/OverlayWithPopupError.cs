using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Warehouse.Mobile;
using Warehouse.Mobile.Extensions;

namespace Warehouse.Droid.Services
{
    public class OverlayWithPopupError : IOverlay
    {
        private readonly IOverlay _overlay;
        private readonly Func<INavigationService> _navigationService;

        public OverlayWithPopupError(IOverlay overlay, Func<INavigationService> navigationService)
        {
            _overlay = overlay;
            _navigationService = navigationService;
        }

        public async Task OverlayAsync(Func<Task> progressAction, string message)
        {
            try
            {
                await _overlay.OverlayAsync(progressAction, message);
            }
            catch (Exception ex)
            {
                _= _navigationService().ShowErrorAsync(ex);
                throw;
            }
        }
    }
}
