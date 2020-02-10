namespace SilverlightStore
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Xml.Linq;

    public class ProductData
    {
        private const string ProductDataCacheKey = "__PRODUCTDATA__";
        private IEnumerable<Product> _products;

        public IEnumerable<Product> Products
        {
            get
            {
                if (this._products == null)
                {
                    this._products = GetOrLoadProducts();
                }
                return this._products;
            }
        }

        private static List<Product> GetOrLoadProducts()
        {
            List<Product> products = HttpContext.Current.Cache[ProductDataCacheKey] as List<Product>;
            if (products == null)
            {
                products = LoadProducts();
                HttpContext.Current.Cache[ProductDataCacheKey] = products;
            }
            return products;
        }

        private static List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();
            XElement xml = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Products.xml"));

            foreach (XElement productElement in xml.Elements("Product"))
            {
                Product p = new Product();

                // Product Info
                {
                    p.Name = productElement.Element("Name").Value;
                    p.Price = decimal.Parse(productElement.Element("Price").Value.Substring(1), CultureInfo.InvariantCulture);
                    p.Summary = productElement.Element("Description").Element("Summary").Value;
                    p.Details = productElement.Element("Description").Element("Details").Value;
                    p.Category = (CategoryType)Enum.Parse(typeof(CategoryType), productElement.Element("Category").Value);
                    p.ImageLarge = ConstructImageUri(productElement.Element("Images").Element("Large").Value, p.Category);
                    p.ImageLogo = ConstructImageUri(productElement.Element("Images").Element("Logo").Value, p.Category);
                    p.ImageSmall = ConstructImageUri(productElement.Element("Images").Element("Small").Value, p.Category);
                    p.PurchaseUrl = new Uri(productElement.Element("PurchaseUrl").Value);
                    p.Permalink = ConstructUri("Product.aspx?Name=" + HttpUtility.UrlEncode(p.Name));
                }

                // SEO Info
                {
                    p.Priority = 0.5f;
                    p.LastModified = DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    p.ChangeFrequency = "monthly";
                }

                products.Add(p);
            }

            return products;
        }

        private static Uri ConstructImageUri(string imageName, CategoryType imageType)
        {
            return ConstructUri("Static/Images/" + imageType.ToString() + "/" + imageName);
        }

        private static Uri ConstructUri(string path)
        {
            return new Uri(GenerateUrl(path));
        }

        private static string GenerateUrl(string suffix)
        {
            return new Uri(HttpContext.Current.Request.Url, suffix).ToString();
        }
    }
}
