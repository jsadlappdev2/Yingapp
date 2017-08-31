using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ying.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayAudio : ContentPage
    {
        public PlayAudio(string URL)
        {
            InitializeComponent();
            audio.Source = URL;
        }
    }
}