﻿using Prism;
using Prism.Ioc;

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
