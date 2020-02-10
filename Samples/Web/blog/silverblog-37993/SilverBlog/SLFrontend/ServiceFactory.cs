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
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SilverBlog.SLFrontend
{
    public class ServiceFactory
    {
        public static BlogService.BlogServiceClient GetClient()
        {
            Binding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(new Uri(Application.Current.Host.Source, "../BlogService.svc"));
            BlogService.BlogServiceClient service = new BlogService.BlogServiceClient(binding, endpoint);
            return service;
        }
        
    }
}
