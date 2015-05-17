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

        public void onCreateButtonClick(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            JsonObject userInfo = new JsonObject();
            userInfo["username"] = JsonValue.CreateStringValue(txtUsername.Text);
            userInfo["first"] = JsonValue.CreateStringValue(txtfName.Text);
            userInfo["last"] = JsonValue.CreateStringValue(txtlName.Text);
            userInfo["password"] = JsonValue.CreateStringValue(txtPassword.Text);
            userInfo["gender"] = JsonValue.CreateStringValue(this.gender.SelectedValue.ToString());
            userInfo["birthday"] = JsonValue.CreateStringValue(birthdayPicker.Date.ToString("yyyy-mm-dd"));
            userInfo["city"] = JsonValue.CreateStringValue(txtCity.Text);
            userInfo["country"] = JsonValue.CreateStringValue(txtCountry.Text);

            D.p(userInfo.ToString());

            WebRequest request = WebRequest.Create(Config.URI);
            request.
            Stream objStream;
            objStream = request.BeginGetResponse()
        }
    }
}
