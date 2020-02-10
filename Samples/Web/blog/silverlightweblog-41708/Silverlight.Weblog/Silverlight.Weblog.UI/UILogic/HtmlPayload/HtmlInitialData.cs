using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Events;
using Silverlight.Weblog.Client.CoreBL.Events;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.UI.UILogic
{
    public class HtmlInitialData : IHtmlInitialData
    {
        public void Initialize()
        {
            string curContent = GetHostHtmlBlock("BlogPost Content");
            if (!string.IsNullOrEmpty(curContent))
            {
                BlogPost blogPostFromHtml = new BlogPost() { IsInitializedByHtmlPayload = true } ;
                blogPostFromHtml.HTML = curContent;
                IoC.Get<IEventAggregator>().GetEvent<LoadBlogPostHTMLEvent>().Publish(blogPostFromHtml);
            }
        }

        protected virtual string GetHostHtmlBlock(string BlockName)
        {
            string SilverlightHostHtml = HtmlPage.Document.GetElementById("silverlightControlHost")
                                                          .GetProperty("innerHTML")
                                                          .ToString();

            string BeginDiv = @"<!-- Begin " + BlockName + " -->";
            string EndDiv = "<!-- End " + BlockName + " -->";
            int BeginDivIndex = SilverlightHostHtml.IndexOf(BeginDiv) + BeginDiv.Length;
            int EndDivIndex = SilverlightHostHtml.IndexOf(EndDiv);
            string BlockContent =  SilverlightHostHtml.Substring(BeginDivIndex, EndDivIndex - BeginDivIndex);
            string BlockContentWithColumnDotCorrection = BlockContent.Replace(@"&nbsp", @"&nbsp;")
                                                                     .Replace(@"&lt", @"&lt;")
                                                                     .Replace(@"&gt", @"&gt;")
                                                                     .Replace(@"&#160", @"&#160;")
                                                                     .Replace(@"&quot", @"&quot;");

            return BlockContentWithColumnDotCorrection;
        }
    }
}
