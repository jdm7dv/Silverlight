using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Shared.Common.Web.Helpers;

namespace Silverlight.Weblog.UI.Web.Feeds
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Feed : IFeed
    {
        private int numberOfPostsInFeed
        {
            get
            {
                return 5;
            }
        }

        public Feed()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IFeedRequestService FeedRequestService { get; set; }

        [Dependency]
        public IRepository<BlogPost> BlogPostRepository { get; set; }

        [Dependency]
        public IBlogPostService BlogPostService { get; set; }

        [Dependency]
        public IRepository<Category> CategoryRepository { get; set; }

        [Dependency]
        public IRepository<User> UserRepository { get; set; }

        [Dependency]
        public ICategoryService CategoryService { get; set; }

        public SyndicationFeedFormatter GetMainFormat(string format)
        {
            return GetFeed(format, string.Empty, BlogPostRepository.Get().Take(numberOfPostsInFeed).ToList());
        }

        public SyndicationFeedFormatter GetBlog(string format, string username)
        {
            return GetFeed(format, username, BlogPostRepository.Get()
                .Where(b => b.User != null && b.User.Username == username).Take(numberOfPostsInFeed).ToList());
        }

        public SyndicationFeedFormatter GetCategory(string format, string username, string category)
        {
            Category curCategory = CategoryRepository.Get().ToList().FirstOrDefault(c => c.Name == category);

            return GetFeed(format, username,
                    CategoryService.GetTopBlogPostsInCategoryForUser(curCategory, numberOfPostsInFeed, username));
        }

        private SyndicationFeedFormatter GetFeed(string format, string username, IList<BlogPost> blogPosts)
        {
            SyndicationFeed feed = !string.IsNullOrEmpty(username) ?
                                            GetSyndicationFeedFor(username) :
                                            GetWebsiteSyndicationFeed();

            SetSyndicationItems(feed, blogPosts);
            return ReturnFormatter(feed, format);
        }

        private SyndicationFeed GetWebsiteSyndicationFeed()
        {
            string title = string.Format("{0} Blog Feed.", FeedRequestService.WebsiteName);
            SyndicationFeed feed = new SyndicationFeed(title, title, FeedRequestService.WebsiteURLRootUri);
            feed.Description = new TextSyndicationContent(string.Format("{0}. {1}", title, title));

            foreach (User curUser in UserRepository.Get())
                feed.Authors.Add(new SyndicationPerson(curUser.Email, curUser.DisplayName, FeedRequestService.WebsiteURLRoot));

            return feed;
        }

        private void SetSyndicationItems(SyndicationFeed feed, IList<BlogPost> blogPosts)
        {
            List<SyndicationItem> items = new List<SyndicationItem>();
            feed.Items = items;

            if (blogPosts != null)
                foreach (BlogPost curBlogPostAddedToFeed in blogPosts)
                    items.Add(ConvertBlogPostToSyndicationItem(curBlogPostAddedToFeed));
        }

        private SyndicationFeed GetSyndicationFeedFor(string username)
        {
            User curUser = UserRepository.Get().FirstOrDefault(u => u.Username == username);

            if (curUser == null)
                throw new ApplicationException("Unknown username: " + username);

            string title = string.Format("{0} Blog Feed.", curUser.DisplayName);
            SyndicationFeed feed = new SyndicationFeed(curUser.BlogName, title, FeedRequestService.WebsiteURLRootUri);
            feed.Authors.Add(new SyndicationPerson(curUser.Email, curUser.DisplayName, FeedRequestService.WebsiteURLRoot));
            feed.Description = new TextSyndicationContent(string.Format("{0}. {1}", curUser.BlogName, title));
            return feed;
        }

        private SyndicationFeedFormatter ReturnFormatter(SyndicationFeed feed, string format)
        {
            if (format == "rss")
                return new Rss20FeedFormatter(feed);
            else if (format == "atom")
                return new Atom10FeedFormatter(feed);
            else return null;
        }

        private SyndicationItem ConvertBlogPostToSyndicationItem(BlogPost curBlogPostAddedToFeed)
        {
            return new SyndicationItem(curBlogPostAddedToFeed.Title,
                                       curBlogPostAddedToFeed.HTML,
                                       new Uri(FeedRequestService.WebsiteURLRoot + curBlogPostAddedToFeed.Permlink),
                                       curBlogPostAddedToFeed.ID.ToString(),
                                       (DateTimeOffset)curBlogPostAddedToFeed.DateCreated.GetValueOrDefault());
        }
    }

    public interface IFeedRequestService
    {
        Uri WebsiteURLRootUri { get; }
        string WebsiteURLRoot { get; }
        string WebsiteName { get; }
    }

    public class FeedRequestService : IFeedRequestService
    {
        public Uri WebsiteURLRootUri
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return new Uri(WebsiteURLRoot);
            }
        }

        public string WebsiteURLRoot
        {
            get
            {

                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.Request.Url.ToString().RemoveAnythingAfter("feed");
            }
        }

        public string WebsiteName
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.Request.Url.Host;
            }
        }
    }
}
