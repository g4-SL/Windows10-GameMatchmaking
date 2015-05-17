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

                        invitationsList.Items.Add(array[0].GetString() + " VS " + array[1].GetString() + " @ " + data["location"].GetString() + ", " + data["date"].GetString());
                        D.p(result);
                    }
                }
                catch (Exception ex)
                {
                    //statusLabel.Text = "No Internet Connection";
                }
            }
        }
    }
}

    