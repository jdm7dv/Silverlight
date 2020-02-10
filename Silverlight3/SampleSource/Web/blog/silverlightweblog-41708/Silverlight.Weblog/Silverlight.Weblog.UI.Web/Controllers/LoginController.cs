using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.UI.Web.Controllers
{
    public class LoginController : Controller
    {
        [Microsoft.Practices.Unity.Dependency]
        public IUserService UserService { get; set; }

        public LoginController()
        {
            IoC.BuildUp(this);
        }

        public ActionResult Load()
        {
            return View("Login");
        }

        public ActionResult Submit()
        {
            Weblog.Server.DAL.User userToValidate = new User();
            this.TryUpdateModel(userToValidate);

            if (UserService.IsValid(userToValidate.Username, userToValidate.Password))
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(userToValidate.Username, false);
                    return null;
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(userToValidate.Username, false);
                    return new RedirectResult("~/Management");
                }
            }
            else
            {
                ModelState.AddModelError(MvcHelper.GetPropertyName((User u)=> u.Password),  "Authentication Failed.");
                return View("Login", userToValidate);
            }
        }

    }

    public static class MvcHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T, string>> property)
        {
            var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            var propertyName = propertyInfo.Name;

            return propertyName;
        }

    }
}
