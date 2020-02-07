namespace SilverlightStore
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.DomainServices;

    public class Product
    {
        [Key]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }
        public decimal Price { get; set; }
        public CategoryType Category { get; set; }
        public Uri ImageLogo { get; set; }
        public Uri ImageSmall { get; set; }
        public Uri ImageLarge { get; set; }
        public Uri PurchaseUrl { get; set; }
        public Uri Permalink { get; set; }

        // SEO Fields (server-only)
        [Exclude]
        public string LastModified { get; set; }
        [Exclude]
        public string ChangeFrequency { get; set; }
        [Exclude]
        public float Priority { get; set; }
    }
}
