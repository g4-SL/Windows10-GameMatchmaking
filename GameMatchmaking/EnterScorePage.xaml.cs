using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Text;
using System.Net.Http;

namespace GameMatchmaking
{
    public sealed partial class EnterScoresPage : Page
    {
        public EnterScoresPage()
        {
            this.InitializeComponent();
            PopulateHostedGames();
        }

        async private void PopulateHostedGames()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/player/me");
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonObject data = jsonResult["data"].GetObject();
                        JsonArray gameIds = data["games_hosted"].GetArray();

                        foreach(JsonValue value in gameIds)
                        {
                            GetGameJson(value.GetNumber());
                        }

                        D.p(result);
                    }
                }
                catch (Exception ex)
                {
                    //statusLabel.Text = "No Internet Connection";
                }
            }
        }

        async private void GetGameJson(double game_id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);
                try
                {
                    string test = "api/game/game/" + game_id;
                    D.p("URI:" + test);
                    HttpResponseMessage response = await client.GetAsync(test);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        D.p("result: " + result);

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonObject data = jsonResult["data"].GetObject();
                        JsonArray array = data["teams"].GetArray();

                        invitationsList.Items.Add(game_id + ". " + array[0].GetString() + " VS " + array[1].GetString() + " @ " + data["location"].GetString() + ", " + data["date"].GetString() + " ");
                        D.p(result);
                    }
                }
                catch (Exception ex)
                {
                    //statusLabel.Text = "No Internet Connection";
                }
            }
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage));
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            string item = e.ClickedItem.ToString();

            char[] split = new char[1];
            split[0] = '.';
            string[] splittedString = item.Split(split);

            AcceptInvitation(int.Parse(splittedString[0]));
        }

        async private void AcceptInvitation(int game_id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonObject jsonObject = new JsonObject();
                jsonObject["decision"] = JsonValue.CreateStringValue("Accept");
                jsonObject["id"] = JsonValue.CreateNumberValue(game_id);

                byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
                ByteArrayContent content = new ByteArrayContent(byteArray);

                try
                {
                    HttpResponseMessage response = await client.PostAsync("api/game/inviterespond/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        JsonObject jsonResult = JsonObject.Parse(result);
                        
                        statusLabel.Text = jsonResult["data"].GetString();
                        invitationsList.Items.Clear();
                        GetGameJson(game_id);
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
    }
}

    