<%@ Page Title="" EnableViewState="false" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="SilverlightStore.Web.Product" %>
<%@ OutputCache CacheProfile="ProductPage" %>
<%@ Register TagPrefix="ria" Namespace="System.Web.DomainServices.WebControls" Assembly="System.Web.DomainServices.WebControls" %>
<asp:Content ContentPlaceHolderID="SilverlightDownLevelContent" runat="server">
    <div class="product-detail-container">
        <div class="product-detail">
            <asp:DetailsView OnDataBound="DetailsView_OnDataBound" BorderColor="#ffffee" BorderWidth="0"
                BorderStyle="None" ID="ProductDetail" DataKeyNames="Name" AutoGenerateRows="false"
                DefaultMode="ReadOnly" runat="server">
                <Fields>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img class="image-logo" src="<%# Eval("ImageLogo")%>" alt="<%# Eval("Name")%>" />
                            <img class="image" src="<%# Eval("ImageLarge")%>" alt="<%# Eval("Name")%>" />
                            <p class="title"><%# Eval("Name") %> - <span class="price"><%# Eval("Price", "{0:c}") %></span></p>
                            <p class="summary"><%# Eval("Summary") %></p>
                            <p class="details"><%# Eval("Details") %></p>
                            <div>
                                <span><a href="<%# Eval("Permalink") %>" target="_blank">Copy Permalink</a></span>
                                <span><a href="<%# Eval("PurchaseUrl") %>" target="_blank">Buy Online</a></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>
        </div>
    </div>
</asp:Content>
