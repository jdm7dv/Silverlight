namespace SilverlightStore.Views
{
    using System.Windows;
    using System.Windows.Browser;
    using SilverlightStore.Services;
    using SilverlightStore.ViewModels;

    public sealed partial class ProductDetail : ViewChildWindow
    {
        public ProductDetail()
        {
            InitializeComponent();
        }

        public ProductDetail(ProductViewModel productViewModel)
            : this()
        {
            this.ViewModel = productViewModel;
        }

        private new ProductViewModel ViewModel
        {
            get
            {
                return (ProductViewModel)base.ViewModel;
            }
            set
            {
                base.ViewModel = value;
            }
        }

        private void BuyOnlineButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(this.ViewModel.PurchaseUrl, "_blank");
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void PermalinkButton_Click(object sender, RoutedEventArgs e)
        {
            ClipboardService clipboardService = ServiceHelper.FindService<ClipboardService>();
            if (clipboardService != null)
            {
                clipboardService.Contents = this.ViewModel.Permalink.ToString();
            }
        }
    }
}

