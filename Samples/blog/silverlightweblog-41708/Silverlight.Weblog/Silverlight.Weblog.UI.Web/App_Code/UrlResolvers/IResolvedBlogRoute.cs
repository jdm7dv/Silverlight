using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public interface IResolvedBlogRoute
    {
        ActionResult ResolveActionResult(BlogPostController controller);
        IEnumerable<BlogPost> GetRelevantBlogPosts();
    }
}