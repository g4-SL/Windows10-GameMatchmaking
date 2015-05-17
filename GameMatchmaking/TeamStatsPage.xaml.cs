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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameMatchmaking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamStatsPage : Page
    {

        override protected void OnNavigatedTo(NavigationEventArgs e)
        {
            string teamName = (string)e.Parameter;
            D.p("TESTTDGEGDH " + teamName);
            getTeamInfo(teamName);
        }

        public TeamStatsPage()
        {
            this.InitializeComponent();
        }

        async private void getTeamInfo(String teamName)
        {
            while(teamName.Length == 0)
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/team/team/" + teamName);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p("2913eu92" + result);

                        JsonObject res = JsonObject.Parse(result);
                        JsonObject dataArray = res.GetNamedObject("data");

                        txtName.Text = dataArray.GetNamedString("name") + " Statistics";
                        txtNumWin.Text = ((int)dataArray.GetNamedNumber("wins")).ToString();
                        txtNumLoss.Text = ((int)dataArray.GetNamedNumber("losses")).ToString();
                        txtGlobalRank.Text = dataArray.GetNamedString("ranking");
                        txtPointsRatio.Text = dataArray.GetNamedNumber("points_ratio").ToString();
                        txtSportType.Text = dataArray.GetNamedString("sport");
                        txtNumTies.Text = ((int)dataArray.GetNamedNumber("ties")).ToString();

                        JsonArray names = dataArray.GetNamedArray("usernames");
                        foreach (JsonValue name in names)
                            txtPlayers.Items.Add(name.ToString());
                    }
                    else
                        D.p("not successful");
                }
                catch (Exception ex)
                {
                }
            }
        }


        private void onCancelClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage));

        }
    }
}
