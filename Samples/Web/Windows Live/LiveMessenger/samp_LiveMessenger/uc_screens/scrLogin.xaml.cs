using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace samp_LiveMessenger
{
	public partial class scrLogin : UserControl
	{
        private Popup _popUp;
        private bool _ignorePageMouseDown = false;

        public scrLogin()
		{
			InitializeComponent();

            this._popUp = new Popup();
            txtLogin.ShowPreviousLogins += new CallbackDelegate(extendUserControlCallbacks);

        }


        #region Generic : Extended UserControl Callbacks
        void extendUserControlCallbacks(CallbackEventArgs args)
        {
            switch (args.Action)
            {
                case "ShowPreviousLogins":
                    if (args.Value == "PreInit")
                    {
                        showLoginsPre();
                    }
                    else
                    {
                        showLoginsPost();
                    }
                    break;
            }
        }
        #endregion

        #region Generic : Hyperlink Wire Up
        private void hlk_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton oLink = (HyperlinkButton)sender;

            //oLink.TextDecorations = TextDecorations.Underline;
        }
        private void hlk_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinkButton oLink = (HyperlinkButton)sender;

            //oLink.TextDecorations = null;
        }
        #endregion

        #region Specific : Help Button Events
        private void showHelpPre(object sender, MouseButtonEventArgs e)
        {
            _ignorePageMouseDown = true;

            FrameworkElement visual = butHelp as FrameworkElement;
            GeneralTransform transform = base.TransformToVisual(visual);
            Point point = transform.Transform(new Point(0.0,0.0));


            this._popUp.Child = new popupHelp();
            
            this._popUp.VerticalOffset = Math.Abs(point.Y) + butHelp.Height;
            this._popUp.HorizontalOffset = Math.Abs(point.X);
            this._popUp.IsOpen = true;

            
        }


        private void showHelpPost(object sender, MouseButtonEventArgs e)
        {
            _ignorePageMouseDown = false;
        }
        #endregion

        #region Specific : Status Button Events
        private void showStatusPre(object sender, MouseButtonEventArgs e)
        {
            _ignorePageMouseDown = true;

            FrameworkElement visual = butStatus as FrameworkElement;
            GeneralTransform transform = base.TransformToVisual(visual);
            Point point = transform.Transform(new Point(0.0, 0.0));


            this._popUp.Child = new popupStatus();

            this._popUp.VerticalOffset = Math.Abs(point.Y) + butStatus.Height;
            this._popUp.HorizontalOffset = Math.Abs(point.X);
            this._popUp.IsOpen = true;


        }


        private void showStatusPost(object sender, MouseButtonEventArgs e)
        {
            _ignorePageMouseDown = false;
        }
        #endregion

        #region Specific : Logins Button Events
        private void showLoginsPre()
        {
            _ignorePageMouseDown = true;

            FrameworkElement visual = txtLogin as FrameworkElement;
            GeneralTransform transform = base.TransformToVisual(visual);
            Point point = transform.Transform(new Point(0.0, 0.0));


            this._popUp.Child = new popupLogins();

            this._popUp.VerticalOffset = Math.Abs(point.Y) + txtLogin.Height;
            this._popUp.HorizontalOffset = Math.Abs(point.X);
            this._popUp.IsOpen = true;


        }


        private void showLoginsPost()
        {
            _ignorePageMouseDown = false;
        }
        #endregion

        #region Generic : Page Events
        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_ignorePageMouseDown) return;

            if (this._popUp.Child != null)
            {
                this._popUp.Child = null;
                this._popUp.IsOpen = false;
            }
        }
        #endregion

    }
}