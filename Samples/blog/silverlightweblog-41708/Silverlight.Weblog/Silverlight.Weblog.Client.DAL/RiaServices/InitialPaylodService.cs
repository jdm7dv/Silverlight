using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.CoreBL.Events;
using Silverlight.Weblog.Client.DAL.Events;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.UI.Web.RiaServices;
using System.Linq;
using System.Windows.Ria;

namespace Silverlight.Weblog.Client.DAL.RiaServices
{
    public class InitialPaylodService : IInitialPaylodService
    {
        [Dependency]
        public IBlogDomainContext BlogDomainContext { get; set; }

        public void Load()
        {
            Uri CurrentUri = HtmlPage.Document.DocumentUri;
            BlogDomainContext.Load(BlogDomainContext.InitialDataQuery(CurrentUri),
                                   initial => IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Publish(initial.Entities.First())
                                   , null);
        }
    }
}
