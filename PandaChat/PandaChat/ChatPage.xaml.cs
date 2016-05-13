using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PandaChat
{
    public partial class ChatPage : PhoneApplicationPage
    {
        public static ChatPage chatPage;
        Chatting chat;
        public string name;

        public ChatPage()
        {
            InitializeComponent();
            //make a static connection to be able to access this control
            App.chatPage = this;
            //make a connection to the chat room
            chat = new Chatting(name);
            consolewrite("Connecting...");
            chat.connect();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string name;
            NavigationContext.QueryString.TryGetValue("name", out name);
            this.name = name;
        }

        public void consolewrite(string text)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                console.Text += text + "\n";
            });
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxMsg.Text != "")
            {
                chat.say(textBoxMsg.Text);
                textBoxMsg.Text = "";
            }
        }
    }
}