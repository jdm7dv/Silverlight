using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Text;
using System.ServiceModel.Web;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.UI.Web.Feeds
{
    [ServiceContract]
    [ServiceKnownType(typeof(Atom10FeedFormatter))]
    [ServiceKnownType(typeof(Rss20FeedFormatter))]
    public interface IFeed
    {
        [OperationContract]
        [WebGetAttribute(UriTemplate = "Get/{format}")]
        SyndicationFeedFormatter GetMainFormat(string format);

        [OperationContract]
        [WebGetAttribute(UriTemplate = "Get/{format}/{username}")]
        SyndicationFeedFormatter GetBlog(string format, string username);

        [OperationContract]
        [WebGetAttribute(UriTemplate = "Get/{format}/{username}/{category}")]
        SyndicationFeedFormatter GetCategory(string format, string username, string category);
    }
}
