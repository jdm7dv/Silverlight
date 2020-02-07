using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Weblog.Shared.Common.Exception
{
    public class SilverlightBlogException : System.Exception
    {
        public SilverlightBlogException()
        {
        }

        public SilverlightBlogException(string message) : base(message)
        {
        }

        public SilverlightBlogException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
