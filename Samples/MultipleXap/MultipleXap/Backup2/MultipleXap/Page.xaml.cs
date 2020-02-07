using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

using System.Reflection;
using System.Windows.Resources;
using System.Windows.Threading;

namespace MultipleXap
{
    public partial class Page : UserControl
    {
        private Uri _baseUri;
        private DispatcherTimer _dt = new DispatcherTimer();

        public Page()
        {
            _baseUri = GetBaseUri();
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //we will use a timer here to simulate a 5 second delay in grabing the next xap
            _dt.Interval = new TimeSpan(0, 0, 5);
            _dt.Tick += new EventHandler(dt_Tick);
            _dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            _dt.Stop();

            // go get the next xap file
            Uri addressUri = new Uri(_baseUri, "ClientBin/MultipleXap1.xap");
            System.Net.WebClient wcXap1 = new System.Net.WebClient();
            wcXap1.BaseAddress = _baseUri;
            wcXap1.OpenReadCompleted += new OpenReadCompletedEventHandler(wcXap1_OpenReadCompleted);
            wcXap1.OpenReadAsync(addressUri); 
        }

        void wcXap1_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if ((e.Error == null) && (e.Cancelled == false))
            {
                // Convert the downloaded stream into an assembly
                Assembly a = LoadAssemblyFromXap("MultipleXap1.dll", e.Result);

                /****************************************************************
                 * The following should work  since I did the following
                 * 
                 *   "added a reference to second assembly then specified that  
                 *    the library assembly that is referenced by the application 
                 *    project is not included with the application package
                 *    when the application assembly is built"
                 *    
                 *  This syntax compiles, but DOES NOT run correctly
                 ****************************************************************
                 * MultipleXap1.Page page = new MultipleXap1.Page();
                 ********************************************************/

                object page = a.CreateInstance("MultipleXap1.Page");
                //Insert UserControl into the Primary grid.
                this.LayoutRoot.Children.Add(page as UserControl);
            }
        }

        public Assembly LoadAssemblyFromXap(string relativeUriString, Stream xapPackageStream)
        {
            Uri uri = new Uri(relativeUriString, UriKind.Relative);
            StreamResourceInfo xapPackageSri = new StreamResourceInfo(xapPackageStream, null);
            StreamResourceInfo assemblySri = Application.GetResourceStream(xapPackageSri, uri);

            AssemblyPart assemblyPart = new AssemblyPart();
            Assembly a = assemblyPart.Load(assemblySri.Stream);
            return a;
        }
        private static Uri GetBaseUri()
        {
            Uri baseUri;
            // Required to initialize variables
            String value = System.IO.Path.GetFileName(HtmlPage.Document.DocumentUri.AbsolutePath);
            if (value.Length <= 0)
            {
                value = HtmlPage.Document.DocumentUri.AbsoluteUri;
                if (value.EndsWith("/"))
                {
                    baseUri = new Uri(value, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    baseUri = new Uri(String.Format("{0}/", value), UriKind.RelativeOrAbsolute);
                }
            }
            else
            {
                baseUri = new Uri(HtmlPage.Document.DocumentUri.AbsoluteUri.Replace(value, String.Empty), UriKind.RelativeOrAbsolute);
            }
            return baseUri;
        }

    }
}
