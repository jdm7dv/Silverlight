using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace Resizer
{
    public partial class App : Application
    {
        private double __currentWidth = 0.0;
        private double __currentHeight = 0.0;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Page();

            HtmlPage.Window.AttachEvent("resize", Application_Resize);

            Size size = GetBroswerSize();
            __currentHeight = size.Height;
            __currentWidth = size.Width;
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            HtmlPage.Window.DetachEvent("resize", Application_Resize);

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        private void Application_Resize(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            Size size = GetBroswerSize();
            Page sl = this.RootVisual as Page;
            ScaleTransform myScale = sl.FindName("MyScale") as ScaleTransform;
            myScale.ScaleX *= size.Width / __currentWidth;
            myScale.ScaleY *= size.Height / __currentHeight;
            __currentHeight = size.Height;
            __currentWidth = size.Width;
        }

        private Size GetBroswerSize()
        {
            Size size = new Size();
            if (null != HtmlPage.Window.GetProperty("innerWidth"))
            {
                size.Width = (double)HtmlPage.Window.GetProperty("innerWidth"); // Mozilla
                size.Height = (double)HtmlPage.Window.GetProperty("innerHeight");
            }
            else
            {
                size.Width = (double)HtmlPage.Document.Body.GetProperty("clientWidth"); // IE
                size.Height = (double)HtmlPage.Document.Body.GetProperty("clientWidth");
            }
            return size;
        }
    }
}
