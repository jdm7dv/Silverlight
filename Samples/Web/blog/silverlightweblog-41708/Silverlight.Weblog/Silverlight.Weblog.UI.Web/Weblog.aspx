<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Weblog.aspx.cs"
    Inherits="Silverlight.Weblog.UI.Web.Weblog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%# BlogPost.User.BlogName + " - " + BlogPost.Title %></title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: hidden;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>

    <script type="text/javascript" src="<%= DirectoryPrefix %>Silverlight.js"></script>

</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="source" value="<%= DirectoryPrefix %>ClientBin/Silverlight.Weblog.UI.xap" />
            <param name="onError" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="minRuntimeVersion" value="3.0.40624.0" />
            <param name="autoUpgrade" value="true" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration: none">
                <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
            <div id="Content">
                <!-- Begin BlogPost Content -->
                <% if (BlogPost != null) {%>
                    <h1><%# BlogPost.Title %></h1>
                    <%= BlogPost.HTML%>
                <% } %>
                <!-- End BlogPost Content -->
            </div>
        </object> <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;border: 0px"></iframe>
    </div>
    </form>
</body>
</html>
