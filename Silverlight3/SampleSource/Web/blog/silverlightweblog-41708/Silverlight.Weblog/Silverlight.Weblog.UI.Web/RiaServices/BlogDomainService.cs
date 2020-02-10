using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.ServiceModel;
using System.Web.DomainServices;
using System.Web.DomainServices.Providers;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL.PartialClasses;
using System.Linq;
using System.Web.Ria;
using System.Web.Ria.Data;
using System.Data;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.UI.Web.Controllers;

namespace Silverlight.Weblog.UI.Web.RiaServices
{
    [EnableClientAccess()]
    public class BlogDomainService : LinqToEntitiesDomainService<EfContext>
    {
        public BlogDomainService()
        {
            IoC.BuildUp(this);
        }

        [Dependency]
        public IUserService UserService { get; set; }


        [Dependency]
        public IBlogRouteResolver BlogRouteResolver { get; set; }

        [OperationContract]
        public User InitialData(Uri UriForDataPack)
        {
            User ReturnUser = UserService.GetUserByUserNameOrDefaultUser(BlogRouteResolver.GetUserName(UriForDataPack));

            foreach (BlogPost curBlogPostToSendToClient in BlogRouteResolver.Resolve(UriForDataPack).GetRelevantBlogPosts())
                ReturnUser.Posts.Add(curBlogPostToSendToClient);

            return ReturnUser;
        }

        public IQueryable<BlogPost> GetBlogPostSet(int PrimaryKey)
        {
            throw new NotSupportedException("Not supported.");

            return this.ObjectContext.BlogPostSet.With(b => b.Comments)
                                                 .With(b => b.User)
                                                 .With(b => b.PostCategories)
                                                 .Where(b => b.ID == PrimaryKey);
        }

        public IQueryable<Comment> GetComments()
        {
            throw new NotSupportedException("Not supported. Required by RIA Services for Insert Comment.");
            return this.ObjectContext.CommentSet;
        }

        [Insert]
        public void InsertComments(Comment comment)
        {
            // stop propogation of new BlogPosts
        //    if (comment.BlogPost.ID == 0)
        //        throw new DomainException("Yeah, all your blog posts are belong to us.");

            this.ObjectContext.AddToCommentSet(comment);
        }
    }

}



