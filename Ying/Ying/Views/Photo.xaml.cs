using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ying
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Photo : ContentPage
    {
        public Photo()
        {
            InitializeComponent();
            
        }

        async void CorpPicture(object sender, EventArgs e)
        {

            try
            {
               // await Navigation.PushAsync(new Views.CorpPicturePage());
                 new NavigationPage(new Views.CorpPicturePage());
            }
            catch (Exception ee)
            {
             await   DisplayAlert("Opps","Error: "+ee.Message.ToString(),"OK");

            }
        }
    }
}