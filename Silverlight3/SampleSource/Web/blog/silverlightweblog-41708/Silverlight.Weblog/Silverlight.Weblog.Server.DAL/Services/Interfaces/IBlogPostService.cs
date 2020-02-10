using System.Collections.Generic;

namespace Silverlight.Weblog.Server.DAL.Services.Interfaces
{
    public interface IBlogPostService
    {
        BlogPost Find(int pk);
        BlogPost Get(string permLink, string username);
        void ClearCategoriesOn(BlogPost post);
    }
}