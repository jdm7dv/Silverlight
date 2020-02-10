using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.DAL.PartialClasses;

namespace Silverlight.Weblog.Server.DAL.Services
{
    public class BlogPostService : IBlogPostService
    {
        public BlogPost Find(int pk)
        {
            var ReturnValue = BlogPostRepository.Get().FirstOrDefault(b => b.ID == pk);

            ReturnValue.UserReference.Load();

            return ReturnValue;
        }

        public BlogPost Get(string permLink)
        {
            if (string.IsNullOrEmpty(permLink))
                return null;

            var ReturnValue = BlogPostRepository.Get()
                                                 .With(b => b.PostCategories.Ignore().Category)
                                                 .With(b => b.User)
                                                 .With(b => b.Comments)
                                                 .With(b => b.Comments.Ignore().ChildComments)
                                                 .FirstOrDefault(b => b.Permlink == permLink);
            return ReturnValue;
        }

        public BlogPost Get(string permLink, string username)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(permLink))
                return Get(permLink);

            var ReturnValue = BlogPostRepository.Get()
                                                .With(b => b.PostCategories.Ignore().Category)
                                                .With(b => b.User)
                                                .With(b => b.Comments)
                                                .With(b => b.Comments.Ignore().ChildComments)
                                                .FirstOrDefault(b => b.Permlink == permLink
                                                                  && b.User.Username == username);
            return ReturnValue;
        }

        public void ClearCategoriesOn(BlogPost post)
        {
            post.PostCategories.Load();
            List<PostCategory> PostCategoriesToDelete = post.PostCategories.ToList();
            post.PostCategories.Clear();
            PostCategoriesToDelete.ForEach(a => a.Delete());
        }

        [Dependency]
        public IRepository<BlogPost> BlogPostRepository { get; set; }


    }


}
