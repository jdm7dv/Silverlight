<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="Silverlight.Weblog.Server.UI.Web.Managment.ForeignKeyField" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />