using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.UI.Web.Models;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class SitemapController : Controller
    {
        public SitemapController()
        {
            // TODO: Figure out how to get Unity to init the Controllers/Views
            IoC.BuildUp(this);
        }

        [Dependency]
        public IRepository<BlogPost> BlogPostRepository { get; set; }

        [Dependency]
        public IRepository<User> UserRepository { get; set; }

        public ActionResult Get()
        {
           List<SitemapLink> data  = 
               BlogPostRepository.Get().ToList()
                    .Select(b => new SitemapLink(b.Permlink ?? "Unknown", 
                                                 b.DateCreated.GetValueOrDefault())).ToList();

            data.AddRange(UserRepository.Get().ToList()
                .Select(u => new SitemapLink(string.Format("Feeds/Service.svc/GetBlog/{0}/rss", u.Username),
                                             DateTime.Now)).ToList());


            return new XmlViewResult("Sitemap", data);
        }


    }

    public class XmlViewResult : ViewResult
    {
        public XmlViewResult(string ViewName, object ViewModel)
        {
            this.ViewName = ViewName;
            this.ViewData = new ViewDataDictionary(ViewModel);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            context.HttpContext.Response.ContentType = "text/xml";
        }
    }
}
