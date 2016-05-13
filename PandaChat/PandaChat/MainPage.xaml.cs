using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PandaChat.Resources;

namespace PandaChat
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            connect();
        }

        private void connect()
        {
            if (textBoxName.Text != "")
            {
                textBoxName.Text = textBoxName.Text.Trim();
                NavigationService.Navigate(new Uri("/ChatPage.xaml?name=" + textBoxName.Text, UriKind.Relative));
            }
        }
    }
}