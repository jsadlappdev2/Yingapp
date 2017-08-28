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

            googleapiservice = new DataService.googleapiservice();


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


        async void translatebtndroid_clicked(object sender, EventArgs e)
        {
            if (indicatordroid.IsRunning) return;   
            try
            {

                indicatordroid.IsRunning = true;
                indicatordroid.IsVisible = true;
                //get source text and target language

                //use default Chinese to English

                string target_lag = "";
                target_lag = "zh-CN";

                string textsource = "";
                if (Device.RuntimePlatform == Device.Android)
                {
                    textsource = textLabelDroid.Text.Trim();
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    textsource = textLabeliOS.Text.Trim();

                }

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {
                    

                    q = textsource,
                    target = target_lag
                 

                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");
             
               
                    translatetexteditordroid.Text = result;
               



            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }

            indicatordroid.IsRunning = false;
            indicatordroid.IsVisible = false;


        }


        async void translatebtnios_clicked(object sender, EventArgs e)
        {
            if (indicatorios.IsRunning) return;

            try
            {

                indicatorios.IsRunning = true;
                indicatorios.IsVisible = true;
                //get source text and target language

                //use default Chinese to English

                string target_lag = "";
                target_lag = "zh-CN";

                string textsource = "";
                if (Device.RuntimePlatform == Device.Android)
                {
                    textsource = textLabelDroid.Text.Trim();
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    textsource = textLabeliOS.Text.Trim();

                }

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {


                    q = textsource,
                    target = target_lag


                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");


                translatetexteditorios.Text = result;




            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }

            indicatorios.IsRunning = false;
            indicatorios.IsVisible = false;


        }


        async void readdroid_clicked(object sender, EventArgs e)

        {
            
          

            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == "zh-CN");        
            else
                locale = new CrossLocale { Language = "zh-CN" };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(translatetexteditordroid.Text.ToString(),
                                   crossLocale: locale);


        }



        async void readios_clicked(object sender, EventArgs e)

        {



            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == "zh-CN");
            else
                locale = new CrossLocale { Language = "zh-CN" };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(translatetexteditorios.Text.ToString(),
                                   crossLocale: locale);


        }







    }
}


