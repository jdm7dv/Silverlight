using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using CookComputing.XmlRpc;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Shared.Common.Logging;

namespace Silverlight.Weblog.UI.Web.MetaWeblog
{
    public class MetaWeblogAPI : XmlRpcService, IMetaWeblog, IHttpHandler
    {
        private string WebsiteURLRoot
        {
            get
            {
                if (HttpContext.Current == null)
                    return null; 

                return HttpContext.Current.Request.Url.ToString().Replace("MetaWeblog/MetaWeblogAPI.ashx", string.Empty);
            }
        }

        #region IoC
        public MetaWeblogAPI()
        {
            // Hack to make sure IoC properties are initialized.
            // Since we don't initialize this service, we need to build it up after
            // it's initialized. 
            IoC.BuildUp(this);
        }

        [Dependency()]
        public IUserService UserService { get; set; }

        [Dependency()]
        public IDBContextContainer DBContextContainer { get; set; }

        [Dependency()]
        public ICategoryService CategoryService { get; set; }

        [Dependency()]
        public IRepository<User> UserRepository { get; set; }

        [Dependency()]
        public IRepository<BlogPost> BlogPostRepository { get; set; }

        [Dependency()]
        public IRepository<Server.DAL.Category> CategoryRepository { get; set; }

        [Dependency]
        public IBlogPostService BlogPostService { get; set; }
        #endregion IoC

        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                BlogPost curBlogPost = GetBlogPostBasedOnID(postid);

                BlogPostService.ClearCategoriesOn(curBlogPost);

                ConvertPostToBlogPost(username, curBlogPost, post);

                return true;
            });
        }

        public CategoryInfo[] getCategories(object blogid, string username, string password)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                User user = UserService.GetUserByUserName(username);
                return (from curCategory in CategoryRepository.Get().ToList()
                        select new CategoryInfo
                                             {
                                                 title = curCategory.Name,
                                                 description = curCategory.Name,
                                                 htmlUrl = user.HomeUrl, // TODO: Fix these 2 fields
                                                 rssUrl = user.HomeUrl + "Feeds/rss/" + user.Username + "/" + curCategory.Name,
                                                 categoryid = curCategory.ID.ToString()
                                             }).ToArray();
            });
        }

        public Post getPost(string postid, string username, string password)
        {
            return ExecuteMetaweblogAPI(username, password,
                () => ConvertBlogPostToPost(GetBlogPostBasedOnID(postid)));
        }

        private Post ConvertBlogPostToPost(BlogPost curBlogPost)
        {
            Post curPost = new Post();

            curPost.title = curBlogPost.Title;
            curPost.description = curBlogPost.HTML;
            //curPost.userid = curBlogPost.User.ID.ToString();
            curPost.dateCreated = curBlogPost.DateCreated.GetValueOrDefault();
            curPost.categories = (from curCategory in curBlogPost.PostCategories
                                  select curCategory.Category.Name).ToArray();
            curPost.permalink = WebsiteURLRoot + curBlogPost.Permlink;
            curPost.link = curPost.permalink;

            return curPost;
        }

        public Post[] getRecentPosts(object blogid, string username, string password, int numberOfPosts)
        {
            return ExecuteMetaweblogAPI(username, password,
                () => (from curBlogPost in BlogPostRepository.Get().Take(numberOfPosts)
                       select ConvertBlogPostToPost(curBlogPost)).ToArray());

        }

        public string newPost(object blogid, string username, string password, Post post, bool publish)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                BlogPost newBlogPost = new BlogPost();

                ConvertPostToBlogPost(username, newBlogPost, post);

                newBlogPost.Create();
                newBlogPost.ForceSave();

                return newBlogPost.ID.ToString();
            });
        }

        private void ConvertPostToBlogPost(string username, BlogPost newBlogPost, Post post)
        {
            newBlogPost.Title = post.title;
            newBlogPost.HTML = post.description;
            newBlogPost.User = UserService.GetUserByUserName(username);
            newBlogPost.SetTitleToPermlink(HttpUtility.UrlDecode(post.title));

            // correct for writer that use invalid dates
            if (post.dateCreated.Year > 2000)
            {
                newBlogPost.DateCreated = post.dateCreated;
                
                // correct for missing hour in windows live writer
                if (post.dateCreated.Year == DateTime.Now.Year
                 && post.dateCreated.Month == DateTime.Now.Month
                 && post.dateCreated.Day == DateTime.Now.Day
                 && post.dateCreated.Hour == 0
                 && post.dateCreated.Minute == 0)
                {
                    newBlogPost.DateCreated = DateTime.Now;
                }
            }
            else
            {
                newBlogPost.DateCreated = DateTime.Now;
            }



            if (post.categories != null)
                foreach (var curCategoryString in post.categories)
                    newBlogPost.PostCategories.Add(new PostCategory(CategoryService.GetCatrgoryOrCreateNewOne(curCategoryString)));
        }



        private void ValidateUser(string username, string password)
        {
            if (!UserService.IsValid(username, password))
            {
                throw new XmlRpcFaultException(0, "Could not authenticate user.");
            }
        }

        // This one Is Rick all the way. 
        public mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                string ImagePhysicalPath = HttpContext.Current.Server.MapPath(@"..\BlogFiles") + @"\";
                string ImageWebPath = string.Format("{0}BlogFiles/", WebsiteURLRoot);
                
                if (mediaobject.bits != null)
                {
                    MemoryStream ms = new MemoryStream(mediaobject.bits);
                    Bitmap bitmap = new Bitmap(ms);

                    ImagePhysicalPath = ImagePhysicalPath + mediaobject.name;
                    string PathOnly = Path.GetDirectoryName(ImagePhysicalPath).Replace("/", "\\");
                    if (!Directory.Exists(PathOnly))
                        Directory.CreateDirectory(PathOnly);

                    bitmap.Save(ImagePhysicalPath);
                }

                mediaObjectInfo mediaInfo = new mediaObjectInfo();
                mediaInfo.url = ImageWebPath + mediaobject.name;

                return mediaInfo;
            });
        }

        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                BlogPost curBlogPost = GetBlogPostBasedOnID(postid);

                curBlogPost.Delete();

                return true;
            });
        }

        private BlogPost GetBlogPostBasedOnID(string postid)
        {
            int Pk = 0;
            if (!int.TryParse(postid, out Pk))
                throw new XmlRpcException("Invalid post id passed: " + postid);

            BlogPost curBlogPost = BlogPostService.Find(Pk);

            if (curBlogPost == null)
                throw new XmlRpcException(string.Format("Blog post with ID {0} was not found", postid));
            return curBlogPost;
        }

        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            return ExecuteMetaweblogAPI(username, password, () =>
            {
                User user = UserService.GetUserByUserName(username);

                if (user == null)
                    throw new XmlRpcException(string.Format("User {0} does not have access to any blogs.", username));

                return new BlogInfo[] { 
                                       new BlogInfo
                                       {
                                           blogid = user.ID.ToString(),
                                           blogName = user.BlogName,
                                           url = WebsiteURLRoot
                                       }
                                   };
            });

        }

        private TResult ExecuteMetaweblogAPI<TResult>(string username, string password, Func<TResult> func)
        {
            try
            {
                ValidateUser(username, password);

                TResult result = func();

                DBContextContainer.CommitChanges();

                return result;
            }
            catch (XmlRpcException)
            {
                throw;
            }
            catch (XmlRpcFaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
#if !DEBUG
                throw new XmlRpcFaultException("Unexpected Error");
#else
                throw;
#endif
            }
        }
    }
}
