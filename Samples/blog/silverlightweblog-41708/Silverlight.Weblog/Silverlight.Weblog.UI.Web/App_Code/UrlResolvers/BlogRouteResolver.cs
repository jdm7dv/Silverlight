using System;
using System.IO;
using System.Security.Policy;
using System.Web;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Shared.Common.Web.Helpers;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class BlogRouteResolver : IBlogRouteResolver
    {
        [Dependency]
        public IBlogPostService BlogPostService { get; set; }

        public IResolvedBlogRoute Resolve(Uri XapUrl)
        {
            return Resolve(GetPermLink(XapUrl), GetUserName(XapUrl));
        }

        /// <summary>
        /// Knows how to retrive the Username from: 
        /// http://foo/VPath/Username/PermLink
        /// -- or --
        /// http://foo/VPath/PermLink
        /// </summary>
        public string GetUserName(Uri XapUrl)
        {
            string[] parsed = GetParsedUrl(XapUrl);

            if (parsed.Length == 2)
                return parsed[0];
            else if (parsed.Length == 1)
                return string.Empty;
            else
                throw new Exception("Url parsing failed.");
        }

        /// <summary>
        /// Knows how to retrive the PermLink from: 
        /// http://foo/VPath/Username/PermLink
        /// -- or --
        /// http://foo/VPath/PermLink
        /// </summary>
        public string GetPermLink(Uri XapUrl)
        {
            string[] parsed = GetParsedUrl(XapUrl);

            if (parsed.Length == 1)
                return parsed[0];
            else if (parsed.Length == 2)
                return parsed[1];
            else
                throw new Exception("Url parsing failed.");
        }

        private string[] GetParsedUrl(Uri XapUrl)
        {
            string VPathUrl = HttpContext.Current.Request.ApplicationPath.Remove(@"/");
            string VPathUrlAndUsernameAndPermlink = XapUrl.GetComponents(UriComponents.Path, UriFormat.UriEscaped);
            string PermLinkAndUsername = VPathUrlAndUsernameAndPermlink.Remove(VPathUrl + "/");
            string[] PermLinkAndUsernameParsed = PermLinkAndUsername.Split('/');

            return PermLinkAndUsernameParsed;
        }

        public IResolvedBlogRoute Resolve(string PermLink, string Username)
        {
            BlogPost blogPost = BlogPostService.Get(PermLink, Username);

            if (blogPost != null)
            {
                return new ResolvedBlogPost(blogPost);
            }
            else
            {
                return new  ResolvedBlogPostList();
            }
        }

    }
}