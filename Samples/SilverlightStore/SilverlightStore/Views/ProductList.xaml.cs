namespace SilverlightStore.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using System.Windows.Ria.Data;
    using SilverlightStore.ViewModels;

    public sealed partial class ProductList : ViewPage
    {
        private bool _productsLoaded; 
        private ChildWindow _activeWindow;
        private ProductsDomainContext _productsDomainContext = new ProductsDomainContext();
        private string _deepLink = string.Empty;

        public ProductList()
        {
            this.InitializeComponent();

            this.LoadProducts();
            this.ViewModel.SelectedProductChanged += this.ViewModel_SelectedProductChanged;
        }

        private new ProductListViewModel ViewModel
        {
            get
            {
                return (ProductListViewModel)base.ViewModel;
            }
        }

        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            base.OnFragmentNavigation(e);
            this.HandleFragmentNavigation(e.Fragment);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string productName;
            if (this.NavigationContext.QueryString.TryGetValue("ProductName", out productName))
            {
                this.HandleFragmentNavigation(productName);
            }
        }

        private void CloseActiveWindow()
        {
            if (this._activeWindow != null)
            {
                this._activeWindow.Close();
                this._activeWindow = null;
            }
        }

        private void HandleFragmentNavigation(string fragment)
        {
            this._deepLink = fragment;

            if (!this._productsLoaded)
            {
                return;
            }

            this.CloseActiveWindow();

            if (string.IsNullOrEmpty(this._deepLink))
            {
                return;
            }

            ProductViewModel productViewModel = this.ViewModel.Products.SingleOrDefault(pvm => pvm.Name == this._deepLink);
            if (productViewModel != null)
            {
                ProductDetail productDetail = new ProductDetail(productViewModel);
                productDetail.Closed += this.ProductDetail_Closed;
                productDetail.Show();

                this._activeWindow = productDetail;
            }
        }

        private void LoadProducts()
        {
            EntityQuery<Product> productsQuery =this._productsDomainContext.GetProductsQuery();
            this._productsDomainContext.Load(productsQuery, this.OnProductsLoaded, null);
        }

        private void OnProductsLoaded(LoadOperation<Product> loadOperation)
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            foreach (Product product in this._productsDomainContext.Products)
            {
                productList.Add(new ProductViewModel(product));
            }
            this.ViewModel.Products = productList;

            this._productsLoaded = true;
            this.HandleFragmentNavigation(this._deepLink);
        }

        private void ViewModel_SelectedProductChanged(object sender, EventArgs e)
        {
            ProductViewModel productViewModel = this.ViewModel.SelectedProduct;
            this.NavigationService.Navigate(new Uri("#" + productViewModel.Name, UriKind.Relative));
        }

        private void ProductDetail_Closed(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("#", UriKind.Relative));
        }
    }
}
