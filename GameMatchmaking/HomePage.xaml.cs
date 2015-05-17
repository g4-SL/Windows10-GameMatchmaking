using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameMatchmaking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            PopulateMyTeams();
        }

        async private void PopulateMyTeams()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/player/me/");
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p(result);

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonObject jsonData = jsonResult["data"].GetObject();
                        JsonArray jsonTeams = jsonData["teams"].GetArray();

                        foreach (JsonValue val in jsonTeams)
                        {
                            teamList.Items.Add(val.GetString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }

        }


        private void createNewTeam(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("create new team");
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CreateTeamPage));
        }

        private void chooseSports(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(SelectSports));
        }

        private void invitationClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(InvitationPage));
        }

        private void createNewTeamClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CreateTeamPage));
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            //teamList.SelectedItem;

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(CreateTeamPage));
        }
    }
}
