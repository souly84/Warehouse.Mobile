using System;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Commands;
using Prism.Common;
using Prism.Navigation;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;
using Xamarin.Forms;

namespace Warehouse.Mobile.UnitTests
{
    public static class ApplicationExtensions
    {
        public static PageNavigationService PageNavigationService(this App app)
        {
            var navigationService = app.Container.Resolve(typeof(INavigationService));
            return navigationService as PageNavigationService;
        }

        public static App ClosePopup(this App app)
        {
            while (app.CurrentViewModel<object>() is CustomPopupMessageViewModel customPopup)
            {
                customPopup.ActionCommand.Execute();
            }
            
            return app;
        }

        public static T CurrentViewModel<T>(this App app)
        {
            return CurrentViewModel<T>(app.PageNavigationService());
        }

        public static T CurrentViewModel<T>(IPageAware pageAware)
        {
            // This is a trick. The problem here is PopupPageNavigationService which clears the IPageAware
            // on navigation back. To Solve the issue we call PageUtilities.GetCurrentPage
            // but it hsould not be like that, all the thing should work automatically
            if (pageAware.Page == null)
            {
                pageAware.Page = PageUtilities.GetCurrentPage(Application.Current.MainPage);
            }
            return (T)pageAware.Page.BindingContext;
        }

        /// <summary>
        /// Sometimes some operations happen in async mode that's why the code should wait for view model.
        /// </summary>
        public static async Task<T> WaitViewModel<T>(this App app)
        {
            Func<bool> waitForViewModel = () => app.CurrentViewModel<object>() is T;
            await waitForViewModel.WaitForAsync(2000);
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
               .ExecuteAsync().Wait();
            return app;
        }

        public static App GoToQuantityToMovePopup(this App app)
        {
            app.GoToPutAway();
            app.CurrentViewModel<PutAwayViewModel>()
               .GoToPopupCommand
               .ExecuteAsync().Wait();
            return app;
        }

        public static App GoToReceptionDetails(this App app)
        {
            app.CurrentViewModel<MenuSelectionViewModel>()
               .GoToAvailableSuppliersCommand
               .ExecuteAsync().Wait();
            app.CurrentViewModel<SelectSupplierViewModel>()
               .Suppliers.First()
               .GoToReceptionDetailsCommand
               .ExecuteAsync().Wait();
            return app;
        }

        public static App GoToSuppliers(this App app)
        {
            app.CurrentViewModel<MenuSelectionViewModel>()
               .GoToAvailableSuppliersCommand
               .ExecuteAsync()
               .Wait();
            return app;
        }
    }
}
