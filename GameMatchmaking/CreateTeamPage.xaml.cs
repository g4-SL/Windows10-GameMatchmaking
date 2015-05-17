using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameMatchmaking
{
    public sealed partial class CreateTeamPage : Page
    {
        public CreateTeamPage()
        {
            this.InitializeComponent();
        }

        private void PopulateSportType()
        {
            // Send http get request for sport types available
        }

        private void onAddTeammateClick(object sender, RoutedEventArgs e)
        {
            // Attempt to add teammate (check with api whether teammate exists)
            teammateListBox.Items.Add(teammateSearchBox.Text);
        }

        private void onCreateClick(object sender, RoutedEventArgs e)
        {
            // Send request with info 

        }
    }
}
