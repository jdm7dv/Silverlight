namespace SilverlightStore.Web
{
    using System;
    using System.Linq;
    using System.Web.UI;

    public partial class Product : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var productName = this.Request.QueryString["Name"];

            if (!string.IsNullOrEmpty(productName))
            {
                // Load product details
                var productData = new ProductData();
                var product = productData.Products.SingleOrDefault(p => p.Name == productName);
                if (product != null)
                {
                    this.ProductDetail.DataSource = new object[] { product };
                    this.ProductDetail.DataBind();
                }
            }
        }

        protected void DetailsView_OnDataBound(object sender, EventArgs e)
        {
            var product = this.ProductDetail.DataItem as SilverlightStore.Product;
            if (product != null)
            {
                this.Title = product.Name;
            }
        }
    }
}
