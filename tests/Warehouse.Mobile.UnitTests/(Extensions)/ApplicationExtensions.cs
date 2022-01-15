using System;
using System.Linq;
using System.Threading.Tasks;
using Prism.Common;
using Prism.Navigation;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;

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

        public static App Scan(this App app, params string[] barcodeData)
        {
            foreach (var barcode in barcodeData)
            {
                app.Scan(
                    new ScanningResult(
                        barcode,
                        "CODE128",
                        DateTime.Now.TimeOfDay
                    )
                );
            }
            return app;
        }

        public static App Scan(this App app, IScanningResult scanningResult)
        {
            app.Scanner.Scan(scanningResult);
            return app;
        }

        public static Task<INavigationResult> GoBackAsync(this App app)
        {
            return app.PageNavigationService().GoBackAsync();
        }
        
        public static App GoToPutAway(this App app)
        {
            app.CurrentViewModel<MenuSelectionViewModel>()
               .GoToPutAwayCommand
               .Execute(null);
            return app;
        }

        public static App GoToReceptionDetails(this App app)
        {
            app.CurrentViewModel<MenuSelectionViewModel>()
                .GoToAvailableSuppliersCommand
                .Execute(null);
            app.CurrentViewModel<SelectSupplierViewModel>()
               .Suppliers.First()
               .GoToReceptionDetailsCommand.Execute();
            return app;
        }

        public static App GoToSuppliers(this App app)
        {
            app.CurrentViewModel<MenuSelectionViewModel>()
               .GoToAvailableSuppliersCommand
               .Execute(null);
            return app;
        }
    }
}
