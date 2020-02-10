using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using System.Linq;

namespace Silverlight.Weblog.UI.Web
{
    using System;
    using System.Web.UI;

    public partial class Sitemap : Page
    {
        public Sitemap()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IRepository<BlogPost> BlogPostRepository { get; set; }

        [Dependency]
        public IRepository<User> UserRepository { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the content-type as XML
            this.Response.ContentType = "text/xml";

            // databind to links
            List<SiteMapData> data  = BlogPostRepository.Get().ToList()
                                        .Select(b => new SiteMapData(b.Permlink ?? "Unknown", 
                                                                     b.DateCreated.GetValueOrDefault())).ToList();
            data.AddRange(UserRepository.Get().ToList()
                .Select(u => new SiteMapData(string.Format("Feeds/Service.svc/GetBlog/{0}/rss", u.Username),
                                             DateTime.Now)).ToList());

            siteMapRepeat.DataSource = data;
            siteMapRepeat.DataBind();
            
        }
    }

    public class SiteMapData
    {
        public SiteMapData(string permlink, DateTime dateCreated)
        {
            this.DateCreated = dateCreated;
            this.Permlink = permlink;
        }

        public string Permlink { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
