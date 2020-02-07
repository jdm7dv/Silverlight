using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Browser;


namespace Chat
{
    public class ChatSession : INotifyPropertyChanged
    {
        public string RemoteUserName { get; set; }
        public string RemoteAvatarUrl { get; internal set; }
        public ObservableCollection<ChatMessage> MessageHistory 
                    { get; internal set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public ChatSession()
        {
            if (HtmlPage.IsEnabled == false)
            {
                PopulateWithDummyData();
            }
             
            
        }

        public void ConnectWithRemoteUser(string remoteUserName)
        {
            // Todo: 1) Wire-Up socket stack to receive notifications of received messages
            //       2) Wire-Up to a remote avatar service instead of hard-coding my picture

            RemoteUserName = remoteUserName;
            RemoteAvatarUrl = "jesse.jpg";
            MessageHistory = new ObservableCollection<ChatMessage>();

            // Notify any listeners of property changes

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("RemoteUserName"));
                PropertyChanged(this, new PropertyChangedEventArgs("RemoteAvatarUrl"));
                PropertyChanged(this, new PropertyChangedEventArgs("MessageHistory"));
            }
        }
        public void SendMessage(string message)
        {
            // Todo: Send to remote chat server over sockets

            MessageHistory.Add(new ChatMessage { Text = message, UserName = "Me" });
        }

        private void PopulateWithDummyData()
        {
            RemoteUserName = "BillG";
            RemoteAvatarUrl = "billg.jpg";

            MessageHistory = new ObservableCollection<ChatMessage>();
            MessageHistory.Add(new ChatMessage { UserName = "BillG", Text = "How is your video going?" });
            MessageHistory.Add(new ChatMessage { UserName = "BillG", Text = "Hmmm....you there?" });
            MessageHistory.Add(new ChatMessage { UserName = "BillG", Text = "Hello???" });
            MessageHistory.Add(new ChatMessage { UserName = "Jesse", Text = "Sorry Bill - working on a video..." });
            MessageHistory.Add(new ChatMessage { UserName = "BillG", Text = "Oh - ok." });
        }



    }
}
