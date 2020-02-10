using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silverlight.Weblog.UI.Web.Models
{
    public class SitemapLink
    {
        public SitemapLink(string permlink, DateTime dateCreated)
        {
            this.DateCreated = dateCreated;
            this.Permlink = permlink;
        }

        public string Permlink { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
