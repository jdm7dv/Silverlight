using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Composite.Events;
using Silverlight.Weblog.Client.DAL.Events;
using Silverlight.Weblog.Client.DAL.ViewModel;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    public class CommentWidgetViewModel : ViewModelBase, ICommentWidgetViewModel, IDisposable, INewCommentPosted
    {
        public CommentWidgetViewModel()
        {
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Subscribe(UpdateViewModel);
        }



        public void UpdateViewModel(User obj)
        {
            this.Model = obj.Posts.First();

            View.IsActive = true;

            RaisePropertyChanged((CommentWidgetViewModel vm) => vm.ChildViewModelComments);

            NewParentlessComment = new NewCommentViewModel(new Comment(), this.Model, this);
        }

        public IEnumerable<SingleCommentViewModel> ChildViewModelComments
        {
            get
            {
                if (this.Model != null)
                    foreach (Comment curComment in this.Model.Comments.Where(c => c.ParentComment == null))
                        yield return new SingleCommentViewModel(curComment, this.Model, this);
            }
        }

        #region Model
        private BlogPost _model;
        public BlogPost Model
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

        #region NewParentlessComment
        private SingleCommentViewModel _newParentlessComment;
        public SingleCommentViewModel NewParentlessComment
        {
            get { return _newParentlessComment; }
            set
            {
                if (_newParentlessComment != value)
                {
                    _newParentlessComment = value;
                    RaisePropertyChanged("NewParentlessComment");
                }

            }
        }
        #endregion NewParentlessComment

        #region Implementation of IDisposable

        public void Dispose()
        {
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Unsubscribe(UpdateViewModel);
        }

        #endregion

        #region Implementation of INewCommentPosted

        public void NewCommentPosted()
        {
            this.RaisePropertyChanged((CommentWidgetViewModel vm) => vm.ChildViewModelComments);
        }

        #endregion
    }

    public interface INewCommentPosted
    {
        void NewCommentPosted();
    }
}