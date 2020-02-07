using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Silverlight.Weblog.Server.UI.Web.Testing
{
    public static class TestExtensions
    {
        public static void AssertIsOfType<TTarget>(this object obj)
        {
            Assert.IsTrue(obj.GetType() == typeof(TTarget), 
                string.Format("{0} is not of type {1}", obj.GetType().FullName, typeof(TTarget).FullName));
        }
    }


}