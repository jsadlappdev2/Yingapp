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
using Ying.DataService;

using Plugin.TextToSpeech;
using System.Net.Http;
using System.Net.Http.Headers;
using static Newtonsoft.Json.JsonConvert;
using Plugin.TextToSpeech.Abstractions;
using Plugin.TextToSpeech;



namespace Ying
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Photo : ContentPage
    {
        ImageSource _imageSource;
        private IMedia _mediaPicker;


        static CrossLocale? locale = null;
        DataService.googleapiservice googleapiservice;
        List<googleapiservice.GoogleTranSource> source;

        public Photo()
        {
            InitializeComponent();
            googleapiservice = new DataService.googleapiservice();
            indicator.IsVisible = false;

        }

        //text detect
        async void detect_clicked(object sender, EventArgs e)
        {
            if (indicator.IsRunning) return;
            try
            {
                string final_text = "";

                //  myisloading.IsLoading = true;
                indicator.IsRunning = true;
                indicator.IsVisible = true;

                //get image into base64stringE:\Study\MobileApps\Xamarin\Yingapp\Ying\Ying\DataService\
                byte[] byteData = App.CroppedImage;
                string base64String = Convert.ToBase64String(byteData);
                string image_base64string = base64String;

                //generate json content string
                string jsonstring_orig = @"{ 
                      
                     ""requests"": [
                           {
                                                 ""image"": {
                                                               ""content"": ""rep_image_base64string""
                                                            },

                                               ""features"": [
                                                            {
                                                              ""type"": ""TEXT_DETECTION""
                                                             }
                                                            ],
                                               ""imageContext"": { ""languageHints"": [""en"" ,""zh-CN"",""zh-TW"" ,""zh""]},


                            }
                    ]
                   }";
                string jsonstring = jsonstring_orig.Replace("rep_image_base64string", image_base64string);

                //new http client and call api

                HttpClient client = new HttpClient();
                var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                var url = "https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js";
                var response = await client.PostAsync(url, content);

                var status = response.IsSuccessStatusCode;
                if (status)
                {

                    string contentString = await response.Content.ReadAsStringAsync();
                    string obj_descrpiton = "";
                    var obj = DeserializeObject<RootObject>(JsonPrettyPrint(contentString));

                    //Get description from textAnnotations
                    foreach (var obj_response in obj.responses)
                    {

                        foreach (var obj_text in obj_response.textAnnotations)
                        {
                            if (obj_text.locale == "en" || obj_text.locale == "zh" || obj_text.locale == "nl")
                            {

                                obj_descrpiton += obj_text.description;
                            }

                        }



                    }

                    final_text = obj_descrpiton;
                    //put into editor
                    detecttexteditor.Text = final_text.ToString();



                }
                else
                {
                    await DisplayAlert("Alert", "Google Detect Text API execute failed.", "OK"); ;

                }


            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google Detect Text API execute failed: " + ee.Message.ToString(), "OK");

            }


            //  myisloading.IsLoading = false;
            indicator.IsRunning = false;
            indicator.IsVisible = false;




        }

        async void translatebtn_clicked(object sender, EventArgs e)
        {
            try
            {
                //get source text and target language

                //use default Chinese to English

                string target_lag = "";
                if (useDefaults.IsToggled)
                {

                    target_lag = "zh-CN";
                }
                else
                {
                    target_lag = "en";

                }

                googleapiservice.GoogleTranSource newItem = new googleapiservice.GoogleTranSource
                {
                    q = detecttexteditor.Text.Trim(),
                    target = target_lag
           
                };

                string result = "";
                result = await googleapiservice.GoogleTranslateAsync(newItem);
                // await DisplayAlert("Alert", "Google translate API execute result: " + result.ToString(), "OK");
                translatetexteditor.Text = result;



            }
            catch (Exception ee)
            {
                await DisplayAlert("Alert", "Google translate API execute Error: " + ee.Message.ToString(), "OK");

            }


        }

        async void readorig_clicked(object sender, EventArgs e)

        {
            string read_lang = "";
            if (useDefaults.IsToggled)
            {

                read_lang = "zh-CN";
            }
            else
            {
                read_lang = "en";

            }

            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
               locale = locales.FirstOrDefault(l => l.ToString() == read_lang);
             //   locale = new CrossLocale { Language = read_lang };
            else
                locale = new CrossLocale { Language = read_lang };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(detecttexteditor.Text.ToString(),                             
                                   crossLocale: locale);


        }

        async void readtarget_clicked(object sender, EventArgs e)

        {
            string read_lang = "";
            if (useDefaults.IsToggled)
            {

                read_lang = "zh-CN";
            }
            else
            {
                read_lang = "en";

            }

            var locales = CrossTextToSpeech.Current.GetInstalledLanguages();
            if (Device.RuntimePlatform == Device.Android)
                locale = locales.FirstOrDefault(l => l.ToString() == read_lang);
               // locale = new CrossLocale { Language = read_lang };
            else
                locale = new CrossLocale { Language = read_lang };//fine for iOS/WP
            // CrossTextToSpeech.Current.Speak(TextLabel.Text.ToString());
            CrossTextToSpeech.Current.Speak(translatetexteditor.Text.ToString(),
                                   crossLocale: locale);

        }

        private async void takephoto_clicked(object sender, EventArgs e)
        {

            //corpimage.IsVisible = false;
            await TakePicture();
        }

        private async void pickupphoto_clicked(object sender, EventArgs e)
        {
          //  corpimage.IsVisible = false;
            await SelectPicture();
        }



        private void Refresh()
        {
            try
            {
                if (App.CroppedImage != null)
                {
                    Stream stream = new MemoryStream(App.CroppedImage);
                  //  corpimage.Source = ImageSource.FromStream(() => stream);
                    //corpimage.IsVisible = true;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #region Photos

        private async void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            ////RM: hack for working on windows phone? 
            await CrossMedia.Current.Initialize();
            _mediaPicker = CrossMedia.Current;
        }

        private async Task SelectPicture()
        {
            Setup();

            _imageSource = null;

            try
            {

                var mediaFile = await this._mediaPicker.PickPhotoAsync();

                _imageSource = ImageSource.FromStream(mediaFile.GetStream);

                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                byte[] imageAsByte = memoryStream.ToArray();

                await Navigation.PushModalAsync(new CropView(imageAsByte, Refresh));

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task TakePicture()
        {
            Setup();

            _imageSource = null;

            try
            {
                var mediaFile = await this._mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Front
                });

                _imageSource = ImageSource.FromStream(mediaFile.GetStream);

                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                byte[] imageAsByte = memoryStream.ToArray();

                await Navigation.PushModalAsync(new CropView(imageAsByte, Refresh));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion



        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }



        public class Vertex
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingPoly
        {
            public List<Vertex> vertices { get; set; }
        }

        public class TextAnnotation
        {
            public string locale { get; set; }
            public string description { get; set; }
            public BoundingPoly boundingPoly { get; set; }
        }

        public class DetectedLanguage
        {
            public string languageCode { get; set; }
        }

        public class Property
        {
            public List<DetectedLanguage> detectedLanguages { get; set; }
        }

        public class DetectedLanguage2
        {
            public string languageCode { get; set; }
        }

        public class Property2
        {
            public List<DetectedLanguage2> detectedLanguages { get; set; }
        }

        public class Vertex2
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox
        {
            public List<Vertex2> vertices { get; set; }
        }

        public class DetectedLanguage3
        {
            public string languageCode { get; set; }
        }

        public class Property3
        {
            public List<DetectedLanguage3> detectedLanguages { get; set; }
        }

        public class Vertex3
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox2
        {
            public List<Vertex3> vertices { get; set; }
        }

        public class DetectedLanguage4
        {
            public string languageCode { get; set; }
        }

        public class Property4
        {
            public List<DetectedLanguage4> detectedLanguages { get; set; }
        }

        public class Vertex4
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox3
        {
            public List<Vertex4> vertices { get; set; }
        }

        public class DetectedLanguage5
        {
            public string languageCode { get; set; }
        }

        public class DetectedBreak
        {
            public string type { get; set; }
        }

        public class Property5
        {
            public List<DetectedLanguage5> detectedLanguages { get; set; }
            public DetectedBreak detectedBreak { get; set; }
        }

        public class Vertex5
        {
            public int x { get; set; }
            public int y { get; set; }
        }

        public class BoundingBox4
        {
            public List<Vertex5> vertices { get; set; }
        }

        public class Symbol
        {
            public Property5 property { get; set; }
            public BoundingBox4 boundingBox { get; set; }
            public string text { get; set; }
        }

        public class Word
        {
            public Property4 property { get; set; }
            public BoundingBox3 boundingBox { get; set; }
            public List<Symbol> symbols { get; set; }
        }

        public class Paragraph
        {
            public Property3 property { get; set; }
            public BoundingBox2 boundingBox { get; set; }
            public List<Word> words { get; set; }
        }

        public class Block
        {
            public Property2 property { get; set; }
            public BoundingBox boundingBox { get; set; }
            public List<Paragraph> paragraphs { get; set; }
            public string blockType { get; set; }
        }

        public class Page
        {
            public Property property { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public List<Block> blocks { get; set; }
        }

        public class FullTextAnnotation
        {
            public List<Page> pages { get; set; }
            public string text { get; set; }
        }

        public class Respons
        {
            public List<TextAnnotation> textAnnotations { get; set; }
            public FullTextAnnotation fullTextAnnotation { get; set; }
        }

        public class RootObject
        {
            public List<Respons> responses { get; set; }
        }
    }
}


