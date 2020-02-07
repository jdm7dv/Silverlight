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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.Client.CoreBL.Events
{
    public class LoadBlogPostHTMLEvent : CompositePresentationEvent<BlogPost>
    {
    }
}
