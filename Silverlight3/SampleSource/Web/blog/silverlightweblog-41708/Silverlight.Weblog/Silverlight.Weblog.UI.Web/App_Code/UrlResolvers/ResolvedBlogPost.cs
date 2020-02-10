using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class ResolvedBlogPost : IResolvedBlogRoute
    {
        public ResolvedBlogPost(BlogPost post)
        {
            this.BlogPost = post;
        }

        public virtual BlogPost BlogPost { get; set; }

        #region Implementation of IResolvedBlogRoute

        public ActionResult ResolveActionResult(BlogPostController controller)
        {
            return controller.PublicView("BlogPostView", this.BlogPost);
        }

        public IEnumerable<BlogPost> GetRelevantBlogPosts()
        {
            return new List<BlogPost>() {BlogPost};
        }

        #endregion
    }
}