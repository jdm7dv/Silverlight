using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Silverlight.Weblog.UI.Web.Views.Syndication
{
    public class SyndicationView : ContentResult
    {
        public SyndicationView(SyndicationFeedFormatter formatter)
        {
            Formatter = formatter;
        }

        public SyndicationFeedFormatter Formatter { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            this.Content = ReadFormatter(Formatter);
            this.ContentType = "text/xml";

            base.ExecuteResult(context);
        }

        private string ReadFormatter(SyndicationFeedFormatter formatter)
        {
            if (Formatter == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            using (TextWriter textWriter = new StringWriter(sb))
            using (XmlWriter xmlWriter = new XmlTextWriter(textWriter))
            {
                Formatter.WriteTo(xmlWriter);
            }
            return sb.ToString();
        }
    }
}