using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Newtonsoft.Json;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Windows.Phone.UI.Input;
using System.Windows.Media.Imaging;

namespace PandaChat
{
    public partial class MapPage : PhoneApplicationPage
    {

        List<Position> list_of_positions = new List<Position>();
        List<string> list_of_users = new List<string>();

        public MapPage()
        {
            InitializeComponent();
            map.Center = new GeoCoordinate(53.704556, 18.250491);
            map.ZoomLevel = 7;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            downloadFromDatabase();
        }

        private void Webclient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            list_of_positions = JsonConvert.DeserializeObject<List<Position>>(e.Result);
            addMarkers();
        }

        private void downloadFromDatabase()
        {
            WebClient webclient = new WebClient();
            webclient.DownloadStringCompleted += Webclient_DownloadStringCompleted;
            webclient.DownloadStringAsync(new Uri("http://mayumu.bluequeen.tk/kawaiiApi.php?key=supersekretnyklucz"));
        }

        private void addMarkers()
        {
            list_of_users = App.chatPage.chat.list_of_users;
            var image = new Image();
            image.Width = 15;
            image.Height = 15;
            image.Opacity = 100;
            image.Source = new BitmapImage(new Uri("/Resources/pin.png", UriKind.RelativeOrAbsolute));
            foreach (string chatter in list_of_users)
            {
                foreach(Position pos in list_of_positions)
                {
                    if(pos.name == chatter)
                    {
                        var mapOverlay = new MapOverlay();
                        mapOverlay.Content = image;
                        mapOverlay.GeoCoordinate = new GeoCoordinate(pos.latitude, pos.longitude);
                        var mapLayer = new MapLayer();
                        mapLayer.Add(mapOverlay);
                        map.Layers.Add(mapLayer);
                    }
                }
            }
        }
    }

    class Position
    {
        public string name;
        public double longitude;
        public double latitude;
    }
}