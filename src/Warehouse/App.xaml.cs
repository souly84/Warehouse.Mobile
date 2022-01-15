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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Warehouse.Mobile
{
    public partial class App : PrismApplication
    {
        private readonly string _ebSoftServerRootUri;

        public App(IPlatformInitializer initializer)
            : this(initializer, "http://wdc-logitest.eurocenter.be/webservice/apitest.php")
        {
        }

        public App(IPlatformInitializer initializer, string ebSoftServerRootUri)
            : base(initializer)
        {
            _ebSoftServerRootUri = ebSoftServerRootUri;
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
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SelectSupplierView, SelectSupplierViewModel>();
            containerRegistry.RegisterForNavigation<MenuSelectionView>();
            containerRegistry.RegisterForNavigation<ReceptionDetailsView>();
            containerRegistry.RegisterForNavigation<PutAwayView>();
            if (!containerRegistry.IsRegistered<ICompany>())
            {
               containerRegistry.RegisterInstance<ICompany>(
                   new EbSoftCompany(_ebSoftServerRootUri)
               );
            }
        }

        private Task NavigateToMainPageAsync()
        {
            return NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MenuSelectionView)}" );
        }
    }
}
