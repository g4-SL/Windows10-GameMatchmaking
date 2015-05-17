using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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


namespace GameMatchmaking
{

    public class GameID
    {
        public int ID = -1;
    }

    public sealed partial class ResultPage : Page
    {
        TextBlock[] TeamMembersA = null;
        TextBlock[] TeamMembersB = null;

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            TeamMembersA = new TextBlock[5];
            TeamMembersA[0] = member1;
            TeamMembersA[1] = member2;
            TeamMembersA[2] = member3;
            TeamMembersA[3] = member4;
            TeamMembersA[4] = member5;

            TeamMembersB = new TextBlock[5];
            TeamMembersB[0] = member1_Copy;
            TeamMembersB[1] = member2_Copy;
            TeamMembersB[2] = member3_Copy;
            TeamMembersB[3] = member4_Copy;
            TeamMembersB[4] = member5_Copy;
            int i = (int)e.Parameter;
            PopulateVersus(i);
        }

        public ResultPage()
        {
            this.InitializeComponent();
        }

        async private void PopulateVersus(int game_id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/game/game/" + game_id);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p(result);

                        JsonObject jsonResult = JsonObject.Parse(result);
                        JsonObject jsonGameData = jsonResult["data"].GetObject();

                        string TeamA = jsonGameData["teams"].GetArray()[0].GetString();
                        string TeamB = jsonGameData["teams"].GetArray()[1].GetString();

                        this.TeamA.Text = TeamA;
                        this.TeamB.Text = TeamB;

                        GetTeamData();
                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }
        }
        async private void GetTeamData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonObject jsonObject = new JsonObject();
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/team/team/" + TeamA.Text);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p("GetTeamData: " + result);

                        JsonObject jsonResult = JsonObject.Parse(result)["data"].GetObject();
                        D.p("JsonResult: " + jsonResult);
                        JsonArray jsonUsernames = jsonResult["usernames"].GetArray();

                        int i = 0;
                        foreach(JsonValue names in jsonUsernames)
                        {
                            TeamMembersA[i].Text = names.GetString();
                            i++;
                        }

                        RankingA.Text = "Rank: " + jsonResult["ranking"].GetString();
                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonObject jsonObject = new JsonObject();
                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/team/team/" + TeamB.Text);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p("GetTeamData: " + result);

                        JsonObject jsonResult = JsonObject.Parse(result)["data"].GetObject();
                        D.p("JsonResult: " + jsonResult);
                        JsonArray jsonUsernames = jsonResult["usernames"].GetArray();

                        int i = 0;
                        foreach (JsonValue names in jsonUsernames)
                        {
                            TeamMembersB[i].Text = names.GetString();
                            i++;
                        }

                        RankingB.Text = "Rank: " + jsonResult["ranking"].GetString();
                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }

        }
        private void OnOkayClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage), (int)1);
        }
    }
}
