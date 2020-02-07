using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Weblog.Shared.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static object GetProperty(this object obj, string propertyName)
        {
            return getPropInfo(obj, propertyName).GetValue(obj, null);
        }

        public static void SetProperty(this object obj, string propertyName, object propertyValue)
        {
            getPropInfo(obj, propertyName).SetValue(obj, propertyValue, null);
        }

        private static PropertyInfo getPropInfo(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        }

        public static void InitializeProperty(this object obj, string propertyName)
        {
            PropertyInfo propInfo = getPropInfo(obj, propertyName);
            if (propInfo != null)
            {
                propInfo.SetValue(obj, Activator.CreateInstance(propInfo.PropertyType), null);
            }

        }

        public static bool PropertyExists(this object obj, string propertyName)
        {
            return (getPropInfo(obj, propertyName) != null);
        }

        public static Type GetPropertyType(this object obj, string propertyName)
        {
            return getPropInfo(obj, propertyName).PropertyType;
        }
    }
}
