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
using Xamarin.Forms.Xaml;


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

        protected override async void OnInitialized()
        {
            await NavigateToMainPageAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigations
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<SelectSupplierView, SelectSupplierViewModel>();
            containerRegistry.RegisterForNavigation<MenuSelectionView>();
            containerRegistry.RegisterForNavigation<ReceptionDetailsView>();
            containerRegistry.RegisterForNavigation<PutAwayView>();

            //Services
            //containerRegistry.RegisterSingleton<ICentralServiceClient, CentralServiceClient>();
            containerRegistry.RegisterInstance<ICompany>(new EbSoftCompany("http://wdc-logcnt.eurocenter.be/webservice/apiscanning.php"));
            //containerRegistry.RegisterSingleton<ICompany, MockWarehouseCompany>();

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
