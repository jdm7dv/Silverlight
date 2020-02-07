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

namespace Chat
{
    public partial class MainPage : UserControl
    {
        private ChatSession chatSession;
        public MainPage()
        {
            InitializeComponent();

            chatSession = Resources["ChatSessionDS"] as ChatSession;
            chatSession.ConnectWithRemoteUser("Jesse");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            chatSession.SendMessage(InputText.Text);
            InputText.Text = string.Empty;

        }
    }
}
