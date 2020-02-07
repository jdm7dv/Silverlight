using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.DomainServices;

namespace Silverlight.Weblog.Server.DAL
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public class CategoryMetadata
    {
        [Exclude]
        public object Posts;

        [Include]
        [Association("Category_ChildCategories", "ID", "ID", IsForeignKey = false)]
        public object ParentCategory;

        [Include]
        [Association("Category_ChildCategories", "ID", "ID", IsForeignKey = true)]
        public object ChildCategories;
    }
}
