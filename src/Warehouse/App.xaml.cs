using EbSoft.Warehouse.SDK;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Plugin.Popups;
using Prism.Unity;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Mobile.ViewModels;
using Warehouse.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Warehouse.Mobile
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        public INavigationService Navigation => NavigationService;

        public IScanner Scanner => Container.Resolve<IScanner>();

        protected override void OnInitialized()
        {
            InitializeComponent();
            _ = NavigateToMainPageAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigations
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<NavigationPage>();

            //Services
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SelectSupplierView, SelectSupplierViewModel>();
            containerRegistry.RegisterForNavigation<MenuSelectionView>();
            containerRegistry.RegisterForNavigation<ReceptionDetailsView>();
            containerRegistry.RegisterForNavigation<PutAwayView>();
            containerRegistry.RegisterForNavigation<StockMoveView>();
            if (!containerRegistry.IsRegistered<ICompany>())
            {
               containerRegistry.RegisterInstance<ICompany>(
                   new EbSoftCompany("http://wdc-logitest.eurocenter.be/webservice/apitest.php")
               );
            }
        }

        private Task NavigateToMainPageAsync()
        {
            return NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MenuSelectionView)}" );
        }
    }
}
