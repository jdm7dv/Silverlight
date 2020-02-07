using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.Services;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class BlogPostController : Controller
    {
        public BlogPostController()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IBlogRouteResolver BlogRouteResolver { get; set; }

        public ActionResult Show(string PermLink, string Username)
        {
            ViewData["DirectoryPrefix"] = DirectoryPrefix(Username);

            IResolvedBlogRoute result = BlogRouteResolver.Resolve(PermLink, Username);
            return result.ResolveActionResult(this);
        }


        protected string DirectoryPrefix(string Username)
        {
            return (string.IsNullOrEmpty(Username)) ? string.Empty : "../";
        }

        public ActionResult PublicView(string ViewName, object Model)
        {
            return View(ViewName, Model);        
        }
    }
}
