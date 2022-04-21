using Dotnet.Commands;
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

        public Task<INavigationResult> NavigationTask { get; private set; }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationTask = NavigateToMainPageAsync();
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
            containerRegistry.RegisterForNavigation<StockMoveView, StockMoveViewModel>();
            // this registration was missed, is it done by purpose?
            containerRegistry.RegisterForNavigation<QuantityToMovePopupView, QuantityToMovePopupViewModel>();
            containerRegistry.RegisterForNavigation<CustomPopupMessageView>();
            containerRegistry.RegisterForNavigation<HistoryView>();
            if (!containerRegistry.IsRegistered<ICommands>())
            {
                containerRegistry.RegisterInstance(new Commands().Validated());
            }
               
            if (!containerRegistry.IsRegistered<ICompany>())
            {
#if MOCK
                containerRegistry.RegisterInstance<ICompany>(
                    new EbSoftCompany(
                        new WebRequest.Elegant.WebRequest(
                            "http://wdc-logcnt.eurocenter.be/webservice/apiscanning.php",
                            Mock.MockDataComponent.HttpClient()
                        )
                    )
                );
#else
                containerRegistry.RegisterInstance<ICompany>(
                    new EbSoftCompany("http://wdc-logcnt.eurocenter.be/webservice/apiscanning.php")
                    //new EbSoftCompany("http://wdc-logitest.eurocenter.be/webservice/apitest.php")
                );
#endif
            }
        }

        private Task<INavigationResult> NavigateToMainPageAsync()
        {
            return NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MenuSelectionView)}");
        }
    }
}
