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


namespace GameMatchmaking
{
    public sealed partial class SelectSports : Page
    {
        public SelectSports()
        {
            this.InitializeComponent();
            selectdistance.SelectedIndex = 0;
            this.PopulateMyTeams();
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

                        foreach(JsonValue val in jsonTeams)
                        {
                            selectteam.Items.Add(val.GetString());
                        }

                        selectteam.SelectedIndex = 0;

                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }

        }


        async private void onStartGameClick(object sender, RoutedEventArgs e)

        {
            if (String.IsNullOrEmpty(selectteam.SelectedValue.ToString()))
                return;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.URI);

                JsonObject jsonObject = new JsonObject();
                jsonObject["team_name"] = JsonValue.CreateStringValue(selectteam.SelectedValue.ToString());
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
                ByteArrayContent content = new ByteArrayContent(byteArray);
                try
                {
                    HttpResponseMessage response = await client.PostAsync("api/team/matchmake/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        D.p(result);
                    }
                }
                catch (Exception ex)
                {
                    // statusLabel.Text = "No Internet Connection";
                }
            }

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ResultPage), (int)1);
        }

        private void onCancelClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage));
        }
    }
}