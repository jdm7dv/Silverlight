using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Silverlight.Weblog.UI.Web.Feeds
{
    class CustomHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            CustomHost customServiceHost =
                new CustomHost(serviceType, baseAddresses[0]);
            return customServiceHost;
        }

        class CustomHost : ServiceHost
        {
            public CustomHost(Type serviceType, params Uri[] baseAddresses)
                : base(serviceType, baseAddresses)
            { }
            protected override void ApplyConfiguration()
            {
                base.ApplyConfiguration();
            }
        }
    }
}