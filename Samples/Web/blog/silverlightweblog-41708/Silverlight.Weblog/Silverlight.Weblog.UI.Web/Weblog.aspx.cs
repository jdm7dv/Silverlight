using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.UI.Web
{
    public partial class Weblog : System.Web.UI.Page
    {
        public Weblog()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IBlogPostService BlogPostService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PermLink))
            {
                this.BlogPost = BlogPostService.Get(PermLink, Username);

                if (BlogPost != null)
                    this.DataBind();
            }

        }

        public BlogPost BlogPost { get; set; }

        public string PermLink { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// If the username route was used, the XAP file will be at the website root.
        /// And not in the same level as the current directory level. 
        /// </summary>

    }

}
