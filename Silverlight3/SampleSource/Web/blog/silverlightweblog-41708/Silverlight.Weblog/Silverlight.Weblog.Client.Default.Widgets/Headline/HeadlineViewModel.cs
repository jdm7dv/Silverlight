using System;
using Microsoft.Practices.Composite.Events;
using Silverlight.Weblog.Client.DAL.Events;
using Silverlight.Weblog.Client.DAL.ViewModel;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.UI.Web.RiaServices;

namespace Silverlight.Weblog.Client.Default.Widgets
{
    public class HeadlineViewModel : ViewModelBase, IHeadlineViewModel, IDisposable
    {
        public HeadlineViewModel()
        {
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Subscribe(GetData);
        }

        public void GetData(User user)
        {
            this.BlogName = user.BlogName;
            this.BlogSubtitle = user.BlogTitle;
            this.HomeUri = new Uri(user.HomeUrl, UriKind.Absolute);
        }

        #region Implementation of IHeadlineViewModel

        private string _blogName;
        public string BlogName
        {
            get { return _blogName; }
            set
            {
                _blogName = value;
                RaisePropertyChanged("BlogName");
            }
        }

        private string _blogSubtitle;
        public string BlogSubtitle
        {
            get { return _blogSubtitle; }
            set
            {
                _blogSubtitle = value;
                RaisePropertyChanged("BlogSubtitle");
            }
        }

        private Uri _homeUri;
        public Uri HomeUri
        {
            get { return _homeUri; }
            set
            {
                _homeUri = value;
                RaisePropertyChanged("HomeUri");
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            IoC.Get<IEventAggregator>().GetEvent<InitiaPayloadLoadedEvent>().Subscribe(GetData);
        }

        #endregion
    }
}