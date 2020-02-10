using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.UI.Web.Feeds;
using Silverlight.Weblog.UI.Web.Views.Syndication;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class FeedController : Controller
    {
        public FeedController()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IFeed Feed { get; set; }

        public ActionResult GetBlog(string format, string username)
        {
            return new SyndicationView(Feed.GetBlog(format, username));
        }

        public ActionResult GetCategory(string format, string username, string category)
        {
            return new SyndicationView(Feed.GetCategory(format, username, category));
        }

        public ActionResult GetMain(string format)
        {
            return new SyndicationView(Feed.GetMainFormat(format));
        }
    }
}
