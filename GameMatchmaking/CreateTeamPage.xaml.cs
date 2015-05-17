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
        public CreateTeamPage()
        {
            this.InitializeComponent();
            foreach (Sport s in Config.sports)
            {
                sportTypeBox.Items.Add(s.Name);
            }
            sportTypeBox.SelectedItem = "Basketball";
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

                HttpResponseMessage response = await client.PostAsync("api/player/search/", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    D.p(result);

                    JsonObject jsonResult = JsonObject.Parse(result);
                    JsonArray jsonMatchingPlayers = jsonResult["data"].GetArray();
                    foreach (JsonValue o in jsonMatchingPlayers)
                    {
                        teammateListBox.Items.Add(o.GetString());
                    }
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

                HttpResponseMessage response = await client.PutAsync("api/team/team/", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    D.p(result);
                }
            }
        }
    }
}
