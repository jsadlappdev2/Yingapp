using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace Ying.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Help : ContentPage
    {
        public Help()
        {
            InitializeComponent();
        }

        async void checkwifistatus(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                wifialter.Text = "You have connected to WIFI!";
            }
            else
            {
                wifialter.Text = "You didn't connect to WIFI!";
            }

        }


     }
}