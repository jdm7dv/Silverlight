<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Silverlight.Weblog.Server.DAL.User>" %>

<%@ Import Namespace="Silverlight.Weblog.UI.Web.Controllers" %>
<%@ Import Namespace="Silverlight.Weblog.Server.DAL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <div>
        <form action="Login/Submit" method="post">
        <table>
            <tr>
                <td>
                    Username
                </td>
                <td>
                    <%= Html.TextBox(MvcHelper.GetPropertyName((User u) => u.Username))%>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Password
                </td>
                <td>
                    <%= Html.Password(MvcHelper.GetPropertyName((User u) => u.Password))%>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <input type="submit" value="Submit" />
                </td>
            </tr>
        </table>
        </form>
        <div>
            <%= Html.ValidationSummary() %>
        </div>
    </div>
</body>
</html>
