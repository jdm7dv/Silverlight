using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silverlight.Weblog.Shared.Common.Web.Helpers
{
    public static class StringHelper
    {
        public static string RemoveAnythingAfter(this string str, string StringToRemoveEverythingAfter)
        {
            int SearchedStringIndex = str.ToLower().LastIndexOf(StringToRemoveEverythingAfter.ToLower());
            return str.Substring(0, SearchedStringIndex);
        }

        public static string Remove(this string str, string StringToRemove)
        {
            return str.Replace(StringToRemove, string.Empty);
        }
    }
}
