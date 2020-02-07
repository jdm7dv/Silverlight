using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Composite.Events;
using Silverlight.Weblog.Client.CoreBL.EventAggregator;
using Silverlight.Weblog.Client.CoreBL.Events;
using Silverlight.Weblog.Client.DAL.Events;
using Silverlight.Weblog.Client.DAL.ViewModel;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using System.Linq;
using Silverlight.Weblog.Shared.Common.Web.Helpers;

namespace Silverlight.Weblog.Client.Default.Widgets.ViewModels
{
    public class BlogPostViewModel : ViewModelBase, IBlogPostViewModel, IDisposable
    {
        public BlogPostViewModel()
        {
            IoC.Get<IEventAggregator>().GetEvent<LoadBlogPostHTMLEvent>().Subscribe(UpdateViewModel);
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Subscribe(UpdateViewModel);
        }

        public void UpdateViewModel(User obj)
        {
            this.Model = obj.Posts.First();
            RaisePropertyChanged((BlogPostViewModel b) => b.FormattedBlogPostCreatedDate);
            RaisePropertyChanged((BlogPostViewModel b) => b.AuthorName);
            RaisePropertyChanged((BlogPostViewModel b) => b.FiledUnder);
            RaisePropertyChanged((BlogPostViewModel b) => b.CountComments);
        //    this.Html = string.Format("<h1>{0}</h1>{1}", this.Model.Title, this.Model.HTML);
        }

        public void UpdateViewModel(BlogPost obj)
        {
            this.Model = obj;
            this.Html = this.Model.HTML;
            View.IsActive = true;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            IoC.Get<IEventAggregator>().GetEvent<LoadBlogPostHTMLEvent>().Unsubscribe(UpdateViewModel);
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Unsubscribe(UpdateViewModel);
        }

        #endregion

        #region FormattedBlogPostCreatedDate
        public string FormattedBlogPostCreatedDate
        {
            get
            {
                if (Model == null || !Model.DateCreated.HasValue)
                    return string.Empty;

                return Model.DateCreated.Value.ToString("dddd, MMMM d yyyy");
            }
        }

        public string AuthorName
        {
            get
            {
                if (Model == null || Model.User == null)
                    return string.Empty;

                return Model.User.DisplayName;
            }
        }

        public string FiledUnder
        {
            get
            {
                if (Model == null || Model.PostCategories.Count == 0)
                    return string.Empty;

                return "| Filed under: " + String.Join(", ", Model.PostCategories.Select(c => c.Category.Name).ToArray());
            }
        }

        public string CountComments
        {
            get
            {
                if (Model == null)
                    return string.Empty;

                if (Model.Comments.Any() == false)
                    return "| No Comments";
                else
                    return "| " + Model.Comments.Count() + " Comments";
            } 
        }

        #region Html
        private string _html;
        public string Html
        {
            get { return _html; }
            set
            {
                if (_html != value)
                {
                    _html = value;
                    RaisePropertyChanged("Html");
                }

            }
        }
        #endregion Html
        #endregion FormattedBlogPostCreatedDate

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
    }
}