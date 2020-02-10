namespace SilverlightStore.Web
{
    using System;
    using System.Web.UI;

    public partial class Sitemap : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.ContentType = "text/xml";
        }
    }
}
