using System;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public interface IBlogRouteResolver
    {
        IResolvedBlogRoute Resolve(Uri XapUrl);
        IResolvedBlogRoute Resolve(string PermLink, string Username);

        /// <summary>
        /// Knows how to retrive the Username from: 
        /// http://foo/VPath/Username/PermLink
        /// -- or --
        /// http://foo/VPath/PermLink
        /// </summary>
        string GetUserName(Uri XapUrl);

        /// <summary>
        /// Knows how to retrive the PermLink from: 
        /// http://foo/VPath/Username/PermLink
        /// -- or --
        /// http://foo/VPath/PermLink
        /// </summary>
        string GetPermLink(Uri XapUrl);
    }
}