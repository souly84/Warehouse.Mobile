using EbSoft.Warehouse.SDK;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Unity;
using Warehouse.Core;
using Warehouse.Core.Plugins;
using Warehouse.Droid;
using Warehouse.Droid.Environment;
using Warehouse.Droid.Services;
using Warehouse.Mobile.Interfaces;
using Warehouse.Scanner.SDK;
using Warehouse.Scanner.SDK.Droid;

namespace Warehouse.Mobile.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        private readonly MainActivity _activity;

        public AndroidInitializer(MainActivity activity)
        {
            _activity = activity;
        }

        public void RegisterTypes(IContainerRegistry container)
        {
            var unityContainer = (UnityContainerExtension)container;
            container.RegisterInstance<IScanner>(new BarcodeScanner().Logged());
            container.RegisterInstance<IEnvironment>(new AndroidEnvironment());

            container.RegisterInstance<IOverlay>(
                new OverlayWithPopupError(
                    new AndHudOverlay(_activity),
                    () => unityContainer.Resolve<INavigationService>()
                )
            );
            container.RegisterInstance<IKeyValueStorage>(new SimpleKeyValueStorage("Warehouse_Mobile_Droid"));

            container.RegisterInstance<ICompany>(
                   //new EbSoftCompany(new LoggedWebRequest(new WebRequest.Elegant.WebRequest("http://wdc-logcnt.eurocenter.be/webservice/apiscanning.php")
                   new EbSoftCompany(new LoggedWebRequest(new WebRequest.Elegant.WebRequest("http://wdc-logitest.eurocenter.be/webservice/apitest.php")
               //new EbSoftCompany("http://wdc-logitest.eurocenter.be/webservice/apitest.php")
                )));
            //Services

            //var tracing = (AppCenterTracing)new AppCenterTracing("ac3d4ba2-411b-4ce3-91bb-7ab861e37796").Identify(
            //    physicalDevice.Id()
            //);
            //var tracingHttpClients = tracing.Trace(new HttpClients());
            //container.RegisterInstance(tracingHttpClients);
            //container.RegisterInstance(tracing.Trace(new CommandPool(logger)));
            //container.RegisterInstance(tracing.Trace(_activity.Navigation));
        }
    }
}
