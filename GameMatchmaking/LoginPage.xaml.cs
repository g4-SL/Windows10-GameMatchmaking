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
        private String resourceName = "WeLikeSports";

        public LoginPage()
        {

            this.InitializeComponent();
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            errorLoginMsg.Text = "";
        }

        private void onSignUpClick(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(createAccountPage));
        }

        public void onLoginClick(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.URI + "api/auth/playerlogin");
            D.p(request.ToString());
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            test = GetUserInfoJson();
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);

            while (true)
            {
                if (isLogin.Length != 0 && String.Equals("success", isLogin))
                {
                    var vault = new Windows.Security.Credentials.PasswordVault();
                    vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, txtEmail.Text, txtPassword.Password));
                    var loginCredential = GetCredentialFromLocker();
                    D.p(loginCredential.UserName);

                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(HomePage));
                    return;
                }
                else if (isLogin.Length != 0 && String.Equals("failed", isLogin))
                {
                    D.p("went in failure");
                    errorLoginMsg.Text = "Wrong email or password";
                    return;
                }
            }

        }
        private JsonObject test;
        private String isLogin = "";

        private JsonObject GetUserInfoJson()
        {
            JsonObject userInfo = new JsonObject();
            userInfo["username"] = JsonValue.CreateStringValue(txtEmail.Text);
            userInfo["password"] = JsonValue.CreateStringValue(txtPassword.Password);

            // ultra massive hack
            User.Name = txtEmail.Text;

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
            request.CookieContainer = new CookieContainer();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    String resultHttp = httpWebStreamReader.ReadToEnd();
                    D.p(resultHttp);
                    JsonObject test = JsonObject.Parse(resultHttp);
                    isLogin = test.GetNamedString("status");
                    D.p(isLogin);
                }
            }
            catch (Exception e)
            {
                D.p("Error attempting to login account:");
                if (e.Message.Contains("Bad Request"))
                    isLogin = "failed";
                D.p(e.Message);
                D.p(e.StackTrace.ToString());
            }

        }

        private Windows.Security.Credentials.PasswordCredential GetCredentialFromLocker()
        {
            String defaultUserName;

            Windows.Security.Credentials.PasswordCredential credential = null;

            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            if (credentialList.Count > 0)
            {
                if (credentialList.Count == 1)
                {
                    credential = credentialList[0];
                }
            }

            return credential;
        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
