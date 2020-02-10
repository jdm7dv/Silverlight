namespace SilverlightStore.Views
{
    using System;
    using System.Globalization;
    using System.Windows.Browser;
    using SilverlightStore.Services;

    public partial class Home : ViewPage
    {
        public Home()
        {
            this.InitializeComponent();
            this.Loaded += (s, a) => this.InitializeDeepLinks();
        }

        private void InitializeDeepLinks()
        {
            DeepLinkService deepLinkService = ServiceHelper.FindService<DeepLinkService>();
                
            if (deepLinkService != null && !string.IsNullOrEmpty(deepLinkService.DeepLink))
            {
                this.Frame.Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Products#{0}", HttpUtility.UrlDecode(deepLinkService.DeepLink)), UriKind.Relative);
            }
        }
    }
}
