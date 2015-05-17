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

namespace GameMatchmaking
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void onSignUpClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(createAccountPage));
        }

 /*       private void onLoginClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(HomePage));
        }*/

        public void onLoginClick(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.URI + "api/auth/playerlogin");
            D.p(request.ToString());
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            test = GetUserInfoJson();
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);


           // if (isLogin.Equals("success"))
          //  {
           //     Frame rootFrame = Window.Current.Content as Frame;
          //      rootFrame.Navigate(typeof(HomePage));
          //  }
        }
        private JsonObject test;
        private String isLogin = "";

        private JsonObject GetUserInfoJson()
        {
            JsonObject userInfo = new JsonObject();
            userInfo["username"] = JsonValue.CreateStringValue(txtEmail.Text);
            userInfo["password"] = JsonValue.CreateStringValue(txtPassword.Password);

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

        void GetResponceStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    String resultHttp = httpWebStreamReader.ReadToEnd();
                    D.p(resultHttp);
                    JsonObject test = JsonObject.Parse(resultHttp);
                    isLogin = test.GetNamedString("status");
                    
                }

            }
            catch (Exception e)
            {
                D.p("Error attempting to login account:");
                D.p(e.Message);
                D.p(e.StackTrace.ToString());
            }

        }
    }
}
