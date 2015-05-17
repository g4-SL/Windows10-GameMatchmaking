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
    public sealed partial class InvitationPage : Page
    {
        public InvitationPage()
        {
            this.InitializeComponent();
            /* HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.URI + "api/player/invitations/");
             D.p(request.ToString());
             request.Method = "GET";
             request.ContentType = "application/json; charset=utf-8";
             test = GetUserInfoJson();
             request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);*/
        }



    }
}

    