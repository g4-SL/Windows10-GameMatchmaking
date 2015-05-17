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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameMatchmaking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamStatsPage : Page
    {
        private String txtNumWinCode = "";
        private String txtNumLossCode = "";
        private String txtRankCityCode = "";
        private String txtRankCountryCode = "";
        private String txtPointsRatioCode = "";
        private String txtPlayersCode = "";
        private String txtSportTypeCode = "";
        private String txtNumTiesCode = "";

        public TeamStatsPage()
        {
            this.InitializeComponent();
            getTeamInfo();
            txtNumWin.Text = txtNumWinCode;
            txtNumLoss.Text = txtNumLossCode;
          //  txtRankCity.Text = txtRankCityCode;
          //  txtRankCountry.Text = txtRankCountryCode;
            txtPointsRatio.Text = txtPointsRatioCode;
            txtPlayers.Text = txtPlayersCode;
            txtSportType.Text = txtSportTypeCode;
            txtNumTies.Text = txtNumTiesCode;
        }

        async private void getTeamInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/team/team/");
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        D.p(result);

                        JsonObject res = JsonObject.Parse(result);
                        txtNumWinCode = res.GetNamedString("wins");
                        txtNumLossCode = res.GetNamedString("losses");
                        //txtRankCityCode = res.GetNamedString("status");
                       // txtRankCountryCode = res.GetNamedString("status");
                        txtPointsRatioCode = res.GetNamedString("points_ratio");
                        txtPlayersCode = res.GetNamedString("usernames");
                        txtSportTypeCode = res.GetNamedString("sport");
                        txtNumTiesCode = res.GetNamedString("ties");
                    }
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
