namespace SilverlightStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.DomainServices;
    using System.Web.Ria;

    /// <summary>
    /// The ProductsDomainService domain service class.
    /// </summary>
    [EnableClientAccess]
    public class ProductsDomainService : DomainService
    {
        private ProductData _productData = new ProductData();

        public IEnumerable<Product> GetProducts()
        {
            return this._productData.Products.OrderBy(p => p.Category).OrderBy(p => p.Name);
        }
    }
}