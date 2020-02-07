using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace VideoPlayer
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Load the main control
            Page page = new Page();
            HtmlPage.RegisterScriptableObject("Page", page);
            this.RootVisual = page;

            foreach (var item in e.InitParams)
            {
                #region Check for Media Param for formatting
                if (item.Key.ToLower() == "m")
                {
                    FormatUri(e.InitParams["m"].ToString(), "m", false);
                }
                else if (item.Key.ToLower() == "markerpath")
                {
                    FormatUri(e.InitParams["markerpath"].ToString(), "markerpath", true);
                }
                else
                {
                    this.Resources.Add(item.Key, item.Value);
                }
                #endregion
            }
        }

        private void FormatUri(string mediaParam, string name, bool forceAbsolute)
        {
            Uri mediaUri;

            if (mediaParam.Contains("://"))
            {
                mediaUri = new Uri(mediaParam, UriKind.RelativeOrAbsolute);
            }
            else if(forceAbsolute)
            {
                mediaUri = new Uri(Host.Source, mediaParam);
            }
            else
            {
                mediaUri = new Uri(mediaParam, UriKind.Relative);
            }

            this.Resources.Add(name, mediaUri);
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            throw (e.ExceptionObject);
        }
    }
}
