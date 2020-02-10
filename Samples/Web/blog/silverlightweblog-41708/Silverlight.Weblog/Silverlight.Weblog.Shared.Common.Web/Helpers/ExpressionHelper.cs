using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Silverlight.Weblog.Shared.Common.Web.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T, TReturn>(this Expression<Func<T, TReturn>> property)
        {
            string[] invocationChain = property.ToString().Split('.');
            string[] filtered = invocationChain.Skip(1).Where(s => s != "Ignore()").ToArray();
            return String.Join(".", filtered);
        }

    }
}
