using System;
using System.Web;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.UI.Web
{
    /// <summary>
    /// Http Module to wire up Entity Framework and ASP.Net. 
    /// </summary>
    public class DBContextModule : IHttpModule
    {
        public DBContextModule()
        {
           // IOC isn't up yet :(
            DBContextContainer = new DBContextContainer();
        }

        [Dependency]
        public IDBContextContainer DBContextContainer { get; set; }

        #region IHttpModule Members
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            DBContextContainer.Initialize();
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            DBContextContainer.Dispose();
        }

        #endregion
    }
}
