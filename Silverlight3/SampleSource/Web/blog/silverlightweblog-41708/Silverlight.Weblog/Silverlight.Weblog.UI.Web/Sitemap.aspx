<?xml version="1.0" encoding="UTF-8" ?>

<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="Sitemap.aspx.cs"
    Inherits="Silverlight.Weblog.UI.Web.Sitemap" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.DomainServices.WebControls" Assembly="System.Web.DomainServices.WebControls" %>
<%--

    Define your DataSource.  (If you uncomment the example below, be sure to update the DomainServiceType property.)
    
    Example:
    
    <asp:DomainDataSource runat="server" ID="SitemapDataSource" 
        DomainServiceTypeName="Silverlight.Weblog.UI.Web.ProductsDomainService" SelectMethod="GetProducts" />
    
--%>
<asp:repeater runat="server" ID="siteMapRepeat">
    <HeaderTemplate>
        <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
            <url>
                <loc><%= new Uri(this.Request.Url, this.ResolveUrl("MetaWeblog/MetaWeblogAPI.ashx"))%></loc>
                <lastmod><% = DateTime.Now.ToShortDateString() %></lastmod>
            </url>       
    </HeaderTemplate>
    <ItemTemplate>
            <url>
                <loc><%# new Uri(this.Request.Url,  this.ResolveUrl(Eval("Permlink").ToString()))%></loc>
                <lastmod><%# Eval("DateCreated")%></lastmod>
            </url>
        <%--
        
            Update the <loc> and related values to expose deep-links 
            to search engine crawlers.

            In the example below, a collection of product detail
            deep-links are generated to ensure crawlers visit each
            product using metadata (<lastmod>, <priority>, etc)
            originating from the DataSource.

            See http://www.sitemaps.org for more infomation.
            
        --%>
        <%--
        
            Example:
            
            <url>
                <loc><%= new Uri(this.Request.Url, this.ResolveUrl("Products.aspx?id=")) %><%# HttpUtility.UrlEncode(Eval("ProductID").ToString()) %></loc>
                <lastmod><%# Eval("LastModified")%></lastmod>
                <changefreq><%# Eval("ChangeFrequency")%></changefreq>
                <priority><%# Eval("Priority")%></priority>
            </url>
            
        --%>
    </ItemTemplate>
    <FooterTemplate>
        </urlset>
    </FooterTemplate>
</asp:repeater>
