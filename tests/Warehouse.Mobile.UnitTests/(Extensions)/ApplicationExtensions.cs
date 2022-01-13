using System;
using Prism.Common;
using Prism.Navigation;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests
{
    public static class ApplicationExtensions
    {
        public static PageNavigationService PageNavigationService(this App app)
        {
            var navigationService = app.Container.Resolve(typeof(INavigationService));
            return navigationService as PageNavigationService;
        }

        public static T CurrentViewModel<T>(this App app)
        {
            return (T)(app.PageNavigationService() as IPageAware).Page.BindingContext;
        }

        public static string GetNavigationUriPath(this App app)
        {
            return app.PageNavigationService().GetNavigationUriPath();
        }

        public static void Scan(this App app, string barcodeData)
        {
            (app.Scanner as MockScanner).Scan(
                new ScanningResult(barcodeData, "CODE128", DateTime.Now.TimeOfDay)
            );
        }
    }
}
