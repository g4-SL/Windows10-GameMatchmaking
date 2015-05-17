using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameMatchmaking
{
    public sealed partial class CreateTeamPage : Page
    {
        private String resourceName = "WeLikeSports";

        public CreateTeamPage()
        {
            this.InitializeComponent();
            foreach (Sport s in Config.sports)
            {
                sportTypeBox.Items.Add(s.Name);
            }
            sportTypeBox.SelectedItem = "Basketball";
            if (!String.IsNullOrEmpty(User.Name))
                teammateListBox.Items.Add(User.Name);
        }

        private void PopulateSportType()
        {
            // Send http get request for sport types available
        }

        async private void onAddTeammateClick(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonObject jsonObject = new JsonObject();
                jsonObject["query"] = JsonValue.CreateStringValue(teammateSearchBox.Text);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
                ByteArrayContent content = new ByteArrayContent(byteArray);

                try
                {
                    HttpResponseMessage response = await client.PostAsync("api/player/search/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p(result);

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonArray jsonMatchingPlayers = jsonResult["data"].GetArray();
                        foreach (JsonValue o in jsonMatchingPlayers)
                        {
                            teammateSearchListBox.Items.Add(o.GetString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "No Internet Connection";
                }
            }
        }

        async private void onCreateClick(object sender, RoutedEventArgs e)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonArray playerNames = new JsonArray();
                foreach (string s in teammateListBox.Items)
                {
                    playerNames.Add(JsonValue.CreateStringValue(s));
                }

                JsonObject jsonObject = new JsonObject();
                jsonObject["name"] = JsonValue.CreateStringValue(teamNameBox.Text);
                jsonObject["sport"] = JsonValue.CreateStringValue((string)sportTypeBox.SelectedValue);
                jsonObject["usernames"] = playerNames;

                byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
                ByteArrayContent content = new ByteArrayContent(byteArray);

                try
                {
                    HttpResponseMessage response = await client.PutAsync("api/team/team/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        D.p(result);
                        Frame rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(HomePage));
                    }
                    else
                    {
                        statusLabel.Text = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "No Internet Connection";
                }

            }
            
        }

        private void onCancelClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage));
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            teammateListBox.Items.Add(e.ClickedItem as string);
            D.p(e.ClickedItem.ToString());
        }
    }
}
