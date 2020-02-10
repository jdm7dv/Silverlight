using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL.Services
{
    public class EntityFrameworkRepository<T> : IRepository<T>
    {
        [Dependency]
        public IDBContextContainer DBContextContainer { get; set; }

        public virtual IQueryable<T> Get()
        {
            // initialize context
            object context = DBContextContainer.DBContext;

            // get collection of properties that might be the ObjectQuery on the class
            PropertyInfo propInfo = GetPropertyInfoForObjectQuery(context);

            return (IQueryable<T>)propInfo.GetValue(context, new object[0]);
        }

        //public T GetByKey(int ID)
        //{
        //    PropertyInfo propInfo = GetPropertyInfoForObjectQuery(context);

        //    EfContext x = new EfContext().;
        //}

        private PropertyInfo GetPropertyInfoForObjectQuery(object context)
        {
            var objectCollectionProperties =
                context.GetType().GetProperties().Where(
                    p => p.PropertyType.IsGenericType
                         && p.PropertyType.GetGenericTypeDefinition() == typeof(ObjectQuery<>)
                         && p.PropertyType.GetGenericArguments()[0] == typeof(T)).ToArray();

            if (objectCollectionProperties.Count() == 0)
            {
                throw new Exception("No property of type ObjectQuery<" + typeof(T).FullName + "> was found on " + context.GetType().FullName);
            }

            return objectCollectionProperties[0];
        }

        public void Insert(T t)
        {
            PropertyInfo propInfo = GetPropertyInfoForObjectQuery(DBContextContainer.DBContext);

            DBContextContainer.DBContext.AddObject(propInfo.Name, t);
        }


        #region IRepository<T> Members


        public void Delete(T t)
        {
            DBContextContainer.DBContext.DeleteObject(t);
        }

        #endregion
    }
}