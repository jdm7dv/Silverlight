using System;
using System.IO;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Compilation;
using System.Web.DynamicData;
using System.Web.Routing;
using System.Web.Services.Protocols;
using System.Web.UI;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.UI.Web.Feeds;
using Silverlight.Weblog.UI.Web.MetaWeblog;
using System.Web.Mvc;

namespace Silverlight.Weblog.UI.Web
{
    public class Routes
    {
        public static void Register(RouteCollection table)
        {
            table.IgnoreRoute("{resource}.axd/{*pathInfo}");

            table.MapRoute("SplashImage", "{Permlink*}SplashScreen.png", new SplashScreenImageRouteHandler());

            //table.MapRoute("LoginPageSubmit", "Login/Submit", new { Controller = "Login", Action = "Submit" });
            //table.MapRoute("LoginPage", "Login", new { Controller = "Login", Action = "Load" });
            table.MapRoute("Sitemap", "Sitemap", new { Controller = "Sitemap", Action = "Get" });
            table.MapRoute("Default", "", new { Controller = "BlogPost", Action = "Show", Username = string.Empty, PermLink = String.Empty });

            MetaModel model = new MetaModel();
            model.DynamicDataFolderVirtualPath = "~/Management/DynamicData/";
            model.RegisterContext(typeof(EfContext), new ContextConfiguration() { ScaffoldAllTables = true });
            table.Add(new DynamicDataRoute("Management/{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "List|Details|Edit|Insert" }),
                Model = model
            });

            
            table.MapRoute("MainFeed", "Feed", new { Controller = "Feed", Action = "GetMain", format = "rss" });
            table.MapRoute("MainFeedFormat", "Feed/{format}", new { Controller = "Feed", Action = "GetMain" });
            table.MapRoute("BlogFeed", "Feed/{format}/{username}", new { Controller = "Feed", Action = "GetBlog" });
            table.MapRoute("CategoryFeed", "Feed/{format}/{username}/{category}", new { Controller = "Feed", Action = "GetCategory" });

            table.MapRoute("BlogPosts", "{PermLink}", new { Controller = "BlogPost", Action = "Show", Username = string.Empty });
            table.MapRoute("UserBlogPosts", "{Username}/{PermLink}", new { Controller = "BlogPost", Action = "Show" });
        
        }
    }



    public class ActionUrlHandler : IRouteHandler
    {
        public ActionUrlHandler(Func<RequestContext, IHttpHandler> action)
        {
            Action = action;
        }

        public Func<RequestContext, IHttpHandler> Action { get; set; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return Action(requestContext);
        }
    }

    public class SplashScreenImageRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.ContentType = GetContentType(requestContext.HttpContext.Request.Url.ToString());

            string filepath = requestContext.HttpContext.Server.MapPath("~/SplashScreen/SplashScreen.png");

            requestContext.HttpContext.Response.WriteFile(filepath);
            requestContext.HttpContext.Response.End();

            return null;
        }

        private static string GetContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }
    }
}
