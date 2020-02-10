using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL
{
    public static class PersistExtensions
    {
        public static void Create<T>(this T entity)
            where T : EntityObject
        {
            IoC.Get<IRepository<T>>().Insert(entity);
        }

        public static void Delete<T>(this T entity)
            where T : EntityObject
        {
            IoC.Get<IRepository<T>>().Delete(entity);
        }

        public static void ForceSave<T>(this T entity)
          where T : EntityObject
        {
            IoC.Get<IDBContextContainer>().CommitChanges();
        }


        //public static IQueryable<BlogPost> Get()
        //{
        //    return IoC.Get<IRepository<BlogPost>>().Get();
        //}
    }
}
