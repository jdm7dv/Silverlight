using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.UI.Web.Managment
{
    public partial class Login : System.Web.UI.Page
    {
        [Microsoft.Practices.Unity.Dependency]
        public IUserService UserService { get; set; }

        public Login()
        {
            IoC.BuildUp(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (UserService.IsValid(tbxUsername.Text, tbxPassword.Text))
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(tbxUsername.Text, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(tbxUsername.Text, false);
                }
            }
            else
            {
                lblResult.Text = "Authentication Failed.";
            }
        }
    }
}
