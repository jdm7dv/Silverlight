using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;

namespace Silverlight.Weblog.Server.DAL.Services
{
    public class CategoryService : ICategoryService
    {
        private Comparison<BlogPost> blogPostDateComprar = ((b1, b2) =>
                                                               {
                                                                   if (b1.DateCreated < b2.DateCreated)
                                                                       return -1;
                                                                   if (b1.DateCreated > b1.DateCreated)
                                                                       return 1;
                                                                   else
                                                                       return 0;
                                                               });

        [Dependency]
        public IRepository<Category> CategoryRepository { get; set; }

        public Server.DAL.Category GetCatrgoryOrCreateNewOne(string curCategoryString)
        {
            Server.DAL.Category curCategory =
                CategoryRepository.Get().FirstOrDefault(c => c.Name == curCategoryString);

            if (curCategory == null)
            {
                curCategory = new Server.DAL.Category() { Name = curCategoryString };
                CategoryRepository.Insert(curCategory);
            }
            return curCategory;
        }

        public IList<BlogPost> GetTopBlogPostsInCategoryForUser(Category curCategory, int NumberOfBlogPosts, string Username)
        {
            List<BlogPost> ReturnValues = new List<BlogPost>();

            GetTopBlogPostsInCategory(curCategory, ReturnValues, Username);
            
            ReturnValues.Sort(blogPostDateComprar);

            return ReturnValues.Take(NumberOfBlogPosts).ToList();
        }

        private void GetTopBlogPostsInCategory(Category curCategory, List<BlogPost> ReturnValues, string Username)
        {
            curCategory.Posts.Load();
            curCategory.ChildCategories.Load();

            ReturnValues.AddRange(curCategory.Posts.Where(b =>
                                                              {
                                                                  b.UserReference.Load();
                                                                  return b.User.Username == Username;
                                                              }).Take(5).ToList());

            foreach (Category childCategory in curCategory.ChildCategories)
            {
                if (curCategory != childCategory)
                    GetTopBlogPostsInCategory(childCategory, ReturnValues, Username);
            }
        }
    }


}
