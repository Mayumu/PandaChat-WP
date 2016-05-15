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
using Windows.Devices.Geolocation;

namespace PandaChat
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator geolocator;
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
                if (!(bool)radioButtonLocation.IsChecked)
                {
                    textBoxName.Text = textBoxName.Text.Trim();
                    NavigationService.Navigate(new Uri("/ChatPage.xaml?name=" + textBoxName.Text, UriKind.Relative));
                }
                else
                {
                    getPosition();
                }
            }
        }

        private async void getPosition()
        {
            try
            {
                //get position
                geolocator = new Geolocator();
                Geoposition geoposition = await geolocator.GetGeopositionAsync();
                //send position to db
                WebClient webclient = new WebClient();
                webclient.DownloadStringAsync(new Uri("http://mayumu.bluequeen.tk/kawaiiApi.php?key=supersekretnyklucz&method=add&name="+textBoxName.Text+"&latitude="+geoposition.Coordinate.Latitude.ToString()+"&longitude="+geoposition.Coordinate.Longitude.ToString()));
                //connect to chat
                textBoxName.Text = textBoxName.Text.Trim();
                NavigationService.Navigate(new Uri("/ChatPage.xaml?name=" + textBoxName.Text, UriKind.Relative));
            }
            catch
            {
                MessageBox.Show("GPS or Internet connection error!");
            }
        }
    }
}