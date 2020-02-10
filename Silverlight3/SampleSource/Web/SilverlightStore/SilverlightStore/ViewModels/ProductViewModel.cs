namespace SilverlightStore.ViewModels
{
    using System;
    using System.Globalization;
    using System.Windows.Media.Imaging;

    public class ProductViewModel : ViewModel
    {
        private Product _product;

        public ProductViewModel()
        {
        }

        public ProductViewModel(Product product)
        {
            this.Product = product;
        }

        public Product Product
        {
            get
            {
                return this._product;
            }
            set
            {
                this._product = value;

                this.RaisePropertyChanged("CategoryType");
                this.RaisePropertyChanged("Details");
                this.RaisePropertyChanged("ImageLarge");
                this.RaisePropertyChanged("ImageLogo");
                this.RaisePropertyChanged("Name");
                this.RaisePropertyChanged("Price");
                this.RaisePropertyChanged("Summary");
            }
        }

        public BitmapImage ImageSmall
        {
            get
            {
                if (this._product != null)
                {
                    return new BitmapImage(this._product.ImageSmall);
                }
                return null;
            }
        }

        public BitmapImage ImageLarge
        {
            get
            {
                if (this._product != null)
                {
                    return new BitmapImage(this._product.ImageLarge);
                }
                return null;
            }
        }

        public BitmapImage ImageLogo
        {
            get
            {
                if (this._product != null)
                {
                    return new BitmapImage(this._product.ImageLogo);
                }
                return null;
            }
        }

        public string Price
        {
            get
            {
                if (this._product != null)
                {
                    return string.Format(CultureInfo.CurrentCulture, "{0:c}", this._product.Price);
                }
                return string.Empty;
            }
        }

        public string CategoryType
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.Category.ToString();
                }
                return string.Empty;
            }
        }

        public string Name
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.Name;
                }
                return string.Empty;
            }
        }

        public string Summary
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.Summary;
                }
                return string.Empty;
            }
        }

        public string Details
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.Details;
                }
                return string.Empty;
            }
        }

        public Uri PurchaseUrl
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.PurchaseUrl;
                }
                return null;
            }
        }

        public Uri Permalink
        {
            get
            {
                if (this._product != null)
                {
                    return this._product.Permalink;
                }
                return null;
            }
        }
    }
}
