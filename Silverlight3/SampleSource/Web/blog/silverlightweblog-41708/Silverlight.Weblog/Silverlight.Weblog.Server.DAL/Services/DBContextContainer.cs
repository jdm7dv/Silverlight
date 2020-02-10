using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL
{
    /// <summary>
    /// Stores the Entity Framework context for each Http Request. 
    /// </summary>
    public class DBContextContainer : IDBContextContainer
    {
        public ObjectContext DBContext
        {
            get { return (ObjectContext) HttpContext.Current.Items["_DbContext"]; }
            set { HttpContext.Current.Items["_DbContext"] = value; }
        }

        public void Initialize()
        {
            DBContext = new EfContext();
        }

        public void Dispose()
        {
            DBContext.SaveChanges();
            DBContext.Dispose();
        }

        public void CommitChanges()
        {
            DBContext.SaveChanges();
        }
    }
}
