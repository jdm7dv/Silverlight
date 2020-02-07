using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Events;
using Silverlight.Weblog.Client.CoreBL.Parts;
using Silverlight.Weblog.Client.DAL.RiaServices;
using Silverlight.Weblog.Client.DAL.Services;
using Silverlight.Weblog.Client.DAL.Services.Interfaces;
using Silverlight.Weblog.Client.Default.Widgets;
using Silverlight.Weblog.Client.Default.Widgets.ViewModels;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Shared.Common.IoC;
using Silverlight.Weblog.UI.Web.RiaServices;

namespace Silverlight.Weblog.UI.UILogic
{
    public static class Initializer
    {
        public static void Init()
        {
            RegisterIoC();
            IoC.Get<IHtmlInitialData>().Initialize();
            RegisterComposer();
            LoadInitialData();
        }

        private static void LoadInitialData()
        {
            IoC.Get<IInitialPaylodService>().Load();
        }

        private static void RegisterComposer()
        {
            Composer.AddAssembly(typeof(BlogPostWidget).Assembly);
            IoC.Register<Parts>(new Parts());
        }

        public static void RegisterIoC()
        {
            IoC.Register<IBlogDomainContext>(new BlogDomainContext());
            IoC.Register<IDBContextContainer, DBContextContainer>();
            IoC.Register<ICreate<Comment>, CommentService>();
            IoC.Register<IEventAggregator>(new EventAggregator());
            IoC.Register<IHtmlInitialData, HtmlInitialData>();
            IoC.Register<IBlogPostViewModel, BlogPostViewModel>();
            IoC.Register<IHeadlineViewModel, HeadlineViewModel>();
            IoC.Register<IInitialPaylodService, InitialPaylodService>();
            IoC.Register<ICommentWidgetViewModel, CommentWidgetViewModel>();
            IoC.Register<IUserDetailsService, UserDetailsService>();
        }

      
    }
}
