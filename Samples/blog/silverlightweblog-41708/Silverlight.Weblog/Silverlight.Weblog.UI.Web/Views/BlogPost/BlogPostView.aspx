<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Silverlight.Weblog.Server.DAL.BlogPost>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%= ViewData.Model.User.BlogName + " - " + ViewData.Model.Title%></title>
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

    <script type="text/javascript" src="<%= ViewData["DirectoryPrefix"] %>Silverlight.js"></script>
    <script type="text/javascript">
        function onSourceDownloadProgressChanged(sender, eventArgs) {
            sender.findName("txt1").Text = Math.round((eventArgs.progress * 100)) + "%";
            sender.findName("txt2").Text = Math.round((eventArgs.progress * 100)) + "%";
            sender.findName("txt3").Text = Math.round((eventArgs.progress * 100)) + "%";
            sender.findName("txt4").Text = Math.round((eventArgs.progress * 100)) + "%";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="splashscreensource" value="SplashScreen/SplashScreen.xaml"/>
            <param name="onSourceDownloadProgressChanged" value="onSourceDownloadProgressChanged" />
            <param name="source" value="<%= ViewData["DirectoryPrefix"] %>ClientBin/Silverlight.Weblog.UI.xap" />
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
                <% if (ViewData.Model != null)
                   {%>
                    <h1><%= ViewData.Model.Title %></h1>
                    <%= ViewData.Model.HTML %>
                <% } %>
                <!-- End BlogPost Content -->
            </div>
        </object> <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px;border: 0px"></iframe>
    </div>
    </form>
</body>
</html>