using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class ResolvedBlogPostList : IResolvedBlogRoute
    {
        #region Implementation of IResolvedBlogRoute

        public ActionResult ResolveActionResult(BlogPostController controller)
        {
            return new ContentResult() { Content = "No blog post specified." };
        }

        public IEnumerable<BlogPost> GetRelevantBlogPosts()
        {
            return new List<BlogPost>();
        }

        #endregion
    }
}