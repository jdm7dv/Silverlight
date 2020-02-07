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
using System.IO;

namespace TrustedFileAccess
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
                InstallWarning.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                SampleArea.Visibility = System.Windows.Visibility.Collapsed;
                InstallWarning.Visibility = System.Windows.Visibility.Visible;
                WarningText.Text = "Application must be installed first";
                return;
            }

            // check to make sure we're running OOB
            if (!App.Current.IsRunningOutOfBrowser)
            {
                WarningText.Visibility = Visibility.Visible;
                InstallButton.Visibility = Visibility.Collapsed;
                InstallWarning.Visibility = System.Windows.Visibility.Visible;
                SampleArea.Visibility = System.Windows.Visibility.Collapsed;
            }
            #endregion
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Install();
        }

        private void EnumerateFiles(object sender, RoutedEventArgs e)
        {
            // create a collection to hold the file enumeration
            List<string> videosInFolder = new List<string>();

            // using the file api to enumerate
            // use the SpecialFolder API to get the users low trust "My Document" type folders
            var videos = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            
            // enumerate the folder
            foreach (var item in videos)
            {
                videosInFolder.Add(item);
            }

            // bind the data
            VideoFileListing.ItemsSource = videosInFolder;
        }

        private void WriteFile(object sender, RoutedEventArgs e)
        {
            // create a path that we are looking to write to
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "myapplog.txt");

            // check to see if the file exists and delete if it does
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // use a stream writer to create a file and write to the contents
            StreamWriter fileWriter = File.CreateText(filePath);
            fileWriter.Write(FileContents.Text);
            fileWriter.Close();
        }

        private void ReadFile(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "myapplog.txt");

            if (File.Exists(filePath))
            {
                StreamReader fileReader = File.OpenText(filePath);
                string contents = fileReader.ReadToEnd();
                FileContents.Text = contents;
                fileReader.Close();
            }
        }

        private void DownloadFile(object sender, RoutedEventArgs e)
        {
            //download some rss text
            WebClient rss = new WebClient();
            rss.OpenReadCompleted += new OpenReadCompletedEventHandler(rss_OpenReadCompleted);
            rss.OpenReadAsync(new Uri("http://feeds.timheuer.com/timheuer"));

            // get a music file
            // this function commented out because the MP3 is not in the sample
            // you can put your own MP3 in the folder and change the URL to download it here.
            //WebClient music = new WebClient();
            //music.OpenReadCompleted += new OpenReadCompletedEventHandler(music_OpenReadCompleted);
            //music.OpenReadAsync(new Uri("kalimba.mp3", UriKind.RelativeOrAbsolute));
        }

        void music_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "music.mp3");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // create a buffer using the Stream which is the result of an OpenRead operation
            byte[] buf = new byte[e.Result.Length];
            e.Result.Read(buf, 0, buf.Length);
            e.Result.Close();

            // use the file API to write the bytes to a path.
            File.WriteAllBytes(filePath, buf);
        }

        void rss_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "timheuerrss.xml");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            byte[] buf = new byte[e.Result.Length];
            e.Result.Read(buf, 0, buf.Length);
            e.Result.Close();
            File.WriteAllBytes(filePath, buf);
        }

        
    }
}
