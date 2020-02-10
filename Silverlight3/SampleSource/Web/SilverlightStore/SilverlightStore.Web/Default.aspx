<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SilverlightStore.Web.Default" %>
<%@ OutputCache CacheProfile="HomePage" %>
<%@ Register TagPrefix="ria" Namespace="System.Web.DomainServices.WebControls" Assembly="System.Web.DomainServices.WebControls" %>
<asp:Content ID="DownLevelContent" ContentPlaceHolderID="SilverlightDownLevelContent" runat="server">
    <h2>Products</h2>
    <ria:DomainDataSource runat="server" ID="ProductsDomainDataSource" DomainServiceTypeName="SilverlightStore.ProductsDomainService" SelectMethod="GetProducts" />
    <asp:ListView runat="server" DataSourceID="ProductsDomainDataSource" DataKeyNames="Name">
        <LayoutTemplate>
            <ul class="product-list">
                <asp:PlaceHolder id="itemPlaceholder" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="product">
                <span class="image">
                    <a href="Product.aspx?Name=<%# Eval("Name")%>">
                        <img src="<%# Eval("ImageSmall")%>" alt="<%# Eval("Name")%>" />
                    </a>
                </span>
                <br />
                <span class="name">
                    <a href="Product.aspx?Name=<%# Eval("Name")%>"><%# Eval("Name")%></a>
                </span>
                <span class="summary"><%# Eval("Summary")%></span>
                <span class="price"><%# Eval("Price", "{0:c}")%></span>
            </li>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
