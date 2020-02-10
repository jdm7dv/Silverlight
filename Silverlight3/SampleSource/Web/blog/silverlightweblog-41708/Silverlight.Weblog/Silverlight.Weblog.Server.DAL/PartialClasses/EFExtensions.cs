using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Silverlight.Weblog.Shared.Common.Web.Helpers;

namespace Silverlight.Weblog.Server.DAL.PartialClasses
{
    public static class EFExtensions
    {
        public static ObjectQuery<T> With<T, TReturn>(this ObjectQuery<T> curQuery, Expression<Func<T, TReturn>> property)
            where T : class, IEntityWithRelationships
        {
            return curQuery.Include(property.GetPropertyName());
        }

        public static IQueryable<T> With<T, TReturn>(this IQueryable<T> curQuery, Expression<Func<T, TReturn>> property)
            where T : class, IEntityWithRelationships
        {
            ObjectQuery<T> objectQuery = curQuery as ObjectQuery<T>;
            if (objectQuery == null)
                throw new Exception("With helper method must be used on EF ObjectQuery");

            return objectQuery.With(property);
        }

        /// <summary>
        /// this method gets ignored when analysing the EF Include Path. 
        /// </summary>
        public static T Ignore<T>(this EntityCollection<T> curQuery) 
            where T : class, IEntityWithRelationships
        {
            return null;
        }
    }
}
