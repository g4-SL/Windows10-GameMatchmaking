using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        private void gender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
