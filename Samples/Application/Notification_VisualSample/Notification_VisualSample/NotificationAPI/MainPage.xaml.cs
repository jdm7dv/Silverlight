using System.Windows;
using System.Windows.Controls;

namespace NotificationAPI
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
      Loaded += new RoutedEventHandler(MainPage_Loaded);
    }

    void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
      #region Running State/Installer Checks
      // check to make sure the app is installed
      // if not, only show the installer
      if (App.Current.InstallState == InstallState.Installed)
      {
        InstallButton.Visibility = Visibility.Collapsed;
      }
      else
      {
        CustomNotificationButton.Visibility = Visibility.Collapsed;
        return;
      }

      // check to make sure we're running OOB
      if (!App.Current.IsRunningOutOfBrowser)
      {
        WarningText.Visibility = Visibility.Visible;
        CustomNotificationButton.Visibility = Visibility.Collapsed;
      }
      #endregion
    }

    private void InstallButton_Click(object sender, RoutedEventArgs e)
    {
      App.Current.Install();
    }

    private void CustomNotificationButton_Click(object sender, RoutedEventArgs e)
    {
      // create the nofitication window API
      NotificationWindow notify = new NotificationWindow();

      // creating the content to be in the window
      CustomNotification custom = new CustomNotification();
      custom.Header = "Header Text";
      custom.Text = "Ipsum dolorem didey up thealmian! Isten ein er falomn";

      // set the window content
      notify.Content = custom;

      // displaying the notification
      notify.Show(8000);
    }
  }
}