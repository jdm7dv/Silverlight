using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LoginSilverlight
{
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var wc = new LoginService.LoginClient();
            wc.DoLoginCompleted += new EventHandler<LoginSilverlight.LoginService.DoLoginCompletedEventArgs>(wc_DoLoginCompleted);
            wc.DoLoginAsync(this.login.Text, this.password.Text);
        }

        void wc_DoLoginCompleted(object sender, LoginSilverlight.LoginService.DoLoginCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                LoginService.LoginInfo ui = e.Result;
                if (ui.UserID != 0) // user is valid with userID, do what ever you want
                {
                    Grid root = Application.Current.RootVisual as Grid;
                    if (root != null)
                    {
                        root.Children.RemoveAt(0);
                        root.Children.Add(new Page1());
                    }
                }
                else
                {
                    System.Windows.Browser.HtmlPage.Window.Alert(ui.LoginMessage);
                }
            }
            else
                System.Windows.Browser.HtmlPage.Window.Alert(e.Error.Message);

        }
    }
}
