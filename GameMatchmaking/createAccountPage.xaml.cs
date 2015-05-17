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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameMatchmaking
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class createAccountPage : Page
    {
        public createAccountPage()
        {
            this.InitializeComponent();
        }
        
        private void validateRequiredFields(object sender, SelectionChangedEventArgs e)
        {
            if(txtUsername.Text.Length == 0)
                System.Diagnostics.Debug.WriteLine("empty username");
        }
        
        public void onCreateButtonClick(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.URI + "api/player/player/");
            D.p(request.ToString());
            request.Method = "PUT";
            request.ContentType = "application/json; charset=utf-8";
            test = GetUserInfoJson();
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);

        }
        private JsonObject test;

        private JsonObject GetUserInfoJson()
        {
            JsonObject userInfo = new JsonObject();
            userInfo["username"] = JsonValue.CreateStringValue(txtUsername.Text);

            // ultra massive hacky poop
            User.Name = txtUsername.Text;

            userInfo["first"] = JsonValue.CreateStringValue(txtfName.Text);
            userInfo["last"] = JsonValue.CreateStringValue(txtlName.Text);
            userInfo["password"] = JsonValue.CreateStringValue(txtPassword.Text);

            string gender = (string)this.gender.SelectedValue == "Male" ? "M" : "F";
            userInfo["gender"] = JsonValue.CreateStringValue(gender);
            userInfo["birthday"] = JsonValue.CreateStringValue(birthdayPicker.Date.ToString("yyyy-MM-dd"));
            userInfo["city"] = JsonValue.CreateStringValue(txtCity.Text);
            userInfo["country"] = JsonValue.CreateStringValue(txtCountry.Text);

            D.p(userInfo.ToString());

            
            return userInfo;
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the stream request operation

            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            byte[] byteArray = Encoding.UTF8.GetBytes(test.ToString());

            postStream.Write(byteArray, 0, byteArray.Length);

            //Start the web request
            request.BeginGetResponse(new AsyncCallback(GetResponceStreamCallback), request);
        }

        async void GetResponceStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    D.p(result);
                }

            }
            catch (Exception e)
            {
                D.p("Error attempting to create account:");
                D.p(e.Message);
                D.p(e.StackTrace.ToString());
            }
        }

        private void messageLabel_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
