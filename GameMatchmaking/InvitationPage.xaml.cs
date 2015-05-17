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
    public class Game
    {
        public string TeamA;
        public string TeamB;
        public string Location;
        public string Date;

        public Game(string teamA, string teamB, string location, string date)
        {
            TeamA = teamA;
            TeamB = teamB;
            Location = location;
            Date = date;
        }
    }


    public sealed partial class InvitationPage : Page
    {
        public InvitationPage()
        {
            this.InitializeComponent();
            PopulateInvitations();
        }

        async private void PopulateInvitations()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/player/invitations");
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonArray jsonArray = jsonResult["data"].GetArray();

                        foreach(JsonValue value in jsonArray)
                        {
                            GetGameJson(value.GetNumber());
                        }

                        if (jsonArray.Count == 0)
                        {
                            statusLabel.Text = "You Have No Invitations!";
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

                        JsonArray acceptedPlayers = data["accepted_players"].GetArray();

                        bool isAccepted = false;
                        foreach(JsonValue val in acceptedPlayers)
                        {
                            if (val.GetString() == User.Name)
                            {
                                isAccepted = true;
                                break;
                            }
                        }

                        string acceptedString = isAccepted ? "ACCEPTED" : "";

                        invitationsList.Items.Add(game_id + ". " + array[0].GetString() + " VS " + array[1].GetString() + " @ " + data["location"].GetString() + ", " + data["date"].GetString() + " " + acceptedString);
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

    