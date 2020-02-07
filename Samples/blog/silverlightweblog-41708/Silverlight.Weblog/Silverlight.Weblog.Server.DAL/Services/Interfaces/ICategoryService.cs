using System.Collections.Generic;
using System.Linq;

namespace Silverlight.Weblog.Server.DAL.Services.Interfaces
{
    public interface ICategoryService
    {
        Server.DAL.Category GetCatrgoryOrCreateNewOne(string curCategoryString);
        IList<BlogPost> GetTopBlogPostsInCategoryForUser(Category category, int NumberOfBlogPosts, string Username);
    }
}