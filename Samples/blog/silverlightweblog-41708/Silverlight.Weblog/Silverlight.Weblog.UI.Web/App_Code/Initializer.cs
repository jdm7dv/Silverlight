using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.UI.Web.Controllers;
using Silverlight.Weblog.UI.Web.Feeds;
using Silverlight.Weblog.UI.Web.MetaWeblog;

namespace Silverlight.Weblog.UI.Web
{
    public static class Initializer
    {
        public static void RegisterIoC()
        {
            IoC.Register<IMetaWeblog, MetaWeblogAPI>();
            IoC.Register<IUserService, UserService>();
            IoC.Register<ICategoryService, CategoryService>();
            IoC.Register<IDBContextContainer, DBContextContainer>();
            IoC.Register<IBlogPostService, BlogPostService>();
            IoC.Register<IFeed, Feed>();
            IoC.Register<IFeedRequestService, FeedRequestService>();
            IoC.Register<IBlogRouteResolver, BlogRouteResolver>();
            IoC.Register(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
        }
    }
}