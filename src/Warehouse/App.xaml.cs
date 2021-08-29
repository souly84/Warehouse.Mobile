using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using System.Threading.Tasks;
using Warehouse.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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

        protected override async void OnInitialized()
        {
            await NavigateToMainPageAsync();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigations
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage>();

            //Services
            //containerRegistry.RegisterSingleton<ICentralServiceClient, CentralServiceClient>();
        }

        private async Task NavigateToMainPageAsync()
        {
            var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(LoginPage)}" );
            if (result.Exception != null)
            {
                throw result.Exception;
            }
        }
    }
}
