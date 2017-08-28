using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Ying;
using Ying.Views;
using Ying.Interface;
using Ying.DataService;


using Plugin.TextToSpeech;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;
using Plugin.TextToSpeech.Abstractions;


namespace Ying
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Voice : ContentPage
    {
        public delegate ContentPage GetEditorInstance(string InitialEditorText);
        static public GetEditorInstance EditorFactory;
        static ISpeechToText speechRecognitionInstnace;

        //for translate
        static CrossLocale? locale = null;
        DataService.googleapiservice googleapiservice;
        List<googleapiservice.GoogleTranSource> source;


        public Voice()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                androidLayout.IsVisible = true;
                voiceButton.OnTextChanged += (s) =>
                {
                    textLabelDroid.Text = s;
                };
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                iOSLayout.IsVisible = true;
                this.Content = iOSLayout;
                speechRecognitionInstnace = DependencyService.Get<ISpeechToText>();
                speechRecognitionInstnace.textChanged += OnTextChange;

            }


        }

        public void OnStart(Object sender, EventArgs args)
        {
            speechRecognitionInstnace.Start();
            nameButtonStart.IsEnabled = false;
            nameButtonStop.IsEnabled = true;
        }
        public void OnStop(Object sender, EventArgs args)
        {
            speechRecognitionInstnace.Stop();
            nameButtonStart.IsEnabled = true;
            nameButtonStop.IsEnabled = false;

        }
        public void OnTextChange(object sender, EventArgsVoiceRecognition e)
        {
            textLabeliOS.Text = e.Text;
            if (e.IsFinal)
            {
                nameButtonStart.IsEnabled = true;
            }
        }





    }
}


