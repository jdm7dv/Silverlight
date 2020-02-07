<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<Silverlight.Weblog.UI.Web.Models.SitemapLink>>" %>

<%@ Import Namespace="Silverlight.Weblog.UI.Web.Models" %>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
    <url>
        <loc><%= new Uri(this.Request.Url, this.ResolveUrl("MetaWeblog/MetaWeblogAPI.ashx"))%></loc>
        <lastmod><% = DateTime.Now.ToShortDateString() %></lastmod>
    </url>   
    <% foreach (SitemapLink sitemapLink in ViewData.Model)
       { %>
    <url>
        <loc><%= new Uri(Request.Url, ResolveUrl(@"../../" + sitemapLink.Permlink))%></loc>
        <lastmod><%= sitemapLink.DateCreated.ToShortDateString() + " " + sitemapLink.DateCreated.ToShortTimeString() %></lastmod>
    </url>    
    <% } %>
</urlset>
