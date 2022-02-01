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
            return CurrentViewModel<T>(app.PageNavigationService());
        }

        public static T CurrentViewModel<T>(IPageAware pageAware)
        {
            return (T)pageAware.Page.BindingContext;
        }

        /// <summary>
        /// Sometimes some operations happen in async mode that's why the code should wait for view model.
        /// </summary>
        public static async Task<T> WaitViewModel<T>(this App app)
        {
            Func<bool> waitForNavigationService = () => (app.PageNavigationService() as IPageAware).Page != null;
            await waitForNavigationService.WaitForAsync();
            Func<bool> waitForViewModel = () => app.CurrentViewModel<object>() is QuantityToMovePopupViewModel;
            await waitForViewModel.WaitForAsync();
            return app.CurrentViewModel<T>();
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
                        "EAN13",
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

        public static T Resolve<T>(this App app)
        {
            return (T)app.Container.Resolve(typeof(T));
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

        public static App QuantityToMovePopup(this App app)
        {
            app.GoToPutAway()
               .CurrentViewModel<PutAwayViewModel>()
               .GoToPopupCommand
               .Execute();
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
