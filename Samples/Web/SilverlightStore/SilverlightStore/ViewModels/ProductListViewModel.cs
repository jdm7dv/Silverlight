namespace SilverlightStore.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public class ProductListViewModel : ViewModel
    {
        private IEnumerable<ProductViewModel> _products;
        private ProductViewModel _selectedProduct;

        public ProductListViewModel()
        {
            ((FrameworkElement)App.Current.RootVisual).SizeChanged += (s, a) => this.RaisePropertyChanged("PageWidth");
        }

        public event EventHandler SelectedProductChanged = delegate { };

        public IEnumerable<ProductViewModel> Products
        {
            get
            {
                return this._products;
            }
            set
            {
                this._products = value;
                this.RaisePropertyChanged("Products");
            }
        }

        public ProductViewModel SelectedProduct
        {
            get
            {
                return this._selectedProduct;
            }
            set
            {
                this._selectedProduct = value;
                this.RaisePropertyChanged("SelectedProduct");
                this.SelectedProductChanged(this, EventArgs.Empty);
            }
        }

        public double PageWidth
        {
            get
            {
                return ((FrameworkElement)App.Current.RootVisual).ActualWidth;
            }
        }
    }
}
