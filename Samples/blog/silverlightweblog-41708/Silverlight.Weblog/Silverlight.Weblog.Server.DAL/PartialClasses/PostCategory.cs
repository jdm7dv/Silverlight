using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.DomainServices;

namespace Silverlight.Weblog.Server.DAL
{
    [MetadataType(typeof(PostCagtegoryMetadta))]
    public partial class PostCategory
    {
        public PostCategory()
        {
            
        }

        public PostCategory(Category category)
        {
            this.Category = category;
        }
    }

    public class PostCagtegoryMetadta
    {
        [Include]
        public object Category;
    }
}
