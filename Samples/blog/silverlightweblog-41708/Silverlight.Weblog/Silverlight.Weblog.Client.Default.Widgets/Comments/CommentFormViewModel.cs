using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using Silverlight.Weblog.Client.DAL.ViewModel;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    public class SingleCommentViewModel : ViewModelBase, INewCommentPosted
    {
        public SingleCommentViewModel(Comment comment, BlogPost commentBlogPost, INewCommentPosted parent)
        {
            IoC.BuildUp(this);

            this.CommentBlogPost = commentBlogPost;
            this.Model = comment;
            this.ParentViewModel = parent;

            RaisePropertyChanged((SingleCommentViewModel vm) => vm.ChildViewModelComments);
        }

 

        #region Model
        private Comment _model;
        public Comment Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    RaisePropertyChanged("Model");
                }
            }
        }
        #endregion Model

        public IEnumerable<SingleCommentViewModel> ChildViewModelComments
        {
            get
            {
                if (Model != null)
                    foreach (Comment childComment in Model.ChildComments)
                        yield return new SingleCommentViewModel(childComment, CommentBlogPost, this);
            }
        }

        public Uri LinkUri
        {
            get
            {
                if (Model == null || string.IsNullOrEmpty(Model.Url))
                    return null;

                return new Uri(Model.Url);
            }
        }

        #region DisplayMessage
        private string _displayMessage;
        public string DisplayMessage
        {
            get { return _displayMessage; }
            set
            {
                if (_displayMessage != value)
                {
                    _displayMessage = value;
                    RaisePropertyChanged("DisplayMessage");
                }

            }
        }
        #endregion DisplayMessage

        protected BlogPost CommentBlogPost { get; set; }


        #region NewChildComment
        private SingleCommentViewModel _newChildComment;
        public SingleCommentViewModel NewChildComment
        {
            get
            {
                if (_newChildComment == null && Model != null)
                {
                    _newChildComment = new NewCommentViewModel(new Comment() { ParentComment = Model }, 
                                                               CommentBlogPost,
                                                               this);
                }

                return _newChildComment;
            }
            set
            {
                if (_newChildComment != value)
                {
                    _newChildComment = value;
                    RaisePropertyChanged("NewParentlessComment");
                }

            }
        }
        #endregion NewChildComment

        public INewCommentPosted ParentViewModel { get; set; }

        #region IsExpanderOpened
        private bool _isExpanderOpened = false;
        public bool IsExpanderOpened
        {
            get { return _isExpanderOpened; }
            set
            {
                if (_isExpanderOpened != value)
                {
                    _isExpanderOpened = value;
                    RaisePropertyChanged("IsExpanderOpened");
                }

            }
        }
        #endregion IsExpanderOpened

        public void NewCommentPosted()
        {
            this.IsExpanderOpened = false;
            this.RaisePropertyChanged((SingleCommentViewModel vm) => vm.ChildViewModelComments);
        }
    }
}