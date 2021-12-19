using EbSoft.Warehouse.SDK;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;
using Warehouse.Mobile.Views;
using Xamarin.Forms;

namespace Warehouse.Mobile
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
            InitializeComponent();
        }

        public INavigationService Navigation => NavigationService;

        public IScanner Scanner => Container.Resolve<IScanner>();

        protected override void OnInitialized()
        {
            _ = NavigateToMainPageAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigations
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SelectSupplierView, SelectSupplierViewModel>();
            containerRegistry.RegisterForNavigation<MenuSelectionView>();
            containerRegistry.RegisterForNavigation<ReceptionDetailsView>();
            containerRegistry.RegisterForNavigation<PutAwayView>();
            containerRegistry.RegisterInstance<ICompany>(
                new EbSoftCompany("http://wdc-logitest.eurocenter.be/webservice/apitest.php")
            );
        }

        private async Task NavigateToMainPageAsync()
        {
            var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MenuSelectionView)}" );
            if (result.Exception != null)
            {
                throw result.Exception;
            }
        }
    }
}
