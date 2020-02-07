using System;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Silverlight.Weblog.Client.DAL.RiaServices;
using Silverlight.Weblog.Client.DAL.Services;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    public class NewCommentViewModel : SingleCommentViewModel
    {
        public NewCommentViewModel(Comment comment, BlogPost commentBlogPost, INewCommentPosted parent) : base(comment, commentBlogPost, parent)
        {
            IoC.BuildUp(this);

            if (UserDetailsService.HasUser)
            {
                comment.Name = UserDetailsService.Username;
                comment.Email = UserDetailsService.Email;
                comment.Url = UserDetailsService.Url;
            }

            AddComment = new DelegateCommand<Comment>(AddCommentInvoke);
        }


        public DelegateCommand<Comment> AddComment { get; set; }
        public void AddCommentInvoke(Comment NewComment)
        {
            if (NewComment.Validate())
            {
                NewComment.BlogPost = CommentBlogPost;
                NewCommentService.Create(NewComment);

                UserDetailsService.SaveUserDetails(NewComment.Name, NewComment.Email, NewComment.Url);

                DisplayMessage = "Posted Successfully!";
                ParentViewModel.NewCommentPosted();

                this.Model = new Comment() { ParentComment = NewComment.ParentComment };
            }
            else
            {
                DisplayMessage = "Please fill in your Name, Email and Comments.";
            }
        }

        [Dependency]
        public ICreate<Comment> NewCommentService { get; set; }

        [Dependency]
        public IUserDetailsService UserDetailsService { get; set; }
    }
}