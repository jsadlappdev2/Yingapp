using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Ying.DataService
{
    public class googleapiservice
    {
        HttpClient client = new HttpClient();
        public class GoogleAPIServices
        {
        }
        /// <summary>
        /// post image to Google OCR Detect Text API service async.
        /// </summary>
        /// <returns>Text detected from image async.</returns>
        /// <param name="DetectTextFromIMage">Item to add.</param>
        public async Task<int> GoogleDetectTextFromIMage()
        {
            string jsonstring = @"
                           {
                              ""requests"": [
                                 {
                              ""image"": {
                                   ""source"": {
                                          ""gcsImageUri"": ""https://s3-ap-southeast-2.amazonaws.com/awss3dailylifehelperapp/images/IMG_20170810_212336_1.jpg""
                                          }
                                        },
                              ""features"": [
                                 {
                                 ""type"": ""DOCUMENT_TEXT_DETECTION""
                                 }
                                  ]
                                     }
                                  ]
                                 }";

            // var data = JsonConvert.SerializeObject(jsonstring);        
            var content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            //need to think about the api_key
            var response = await client.PostAsync("https://vision.googleapis.com/v1/images:annotate?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js", content);
            // var result = JsonConvert.DeserializeObject<int>(response.Content.ReadAsStringAsync().Result);
            // return result;
            return 1;

        }


        public class Image
        {
            public string content { get; set; }
        }

        public class Feature
        {
            public string type { get; set; }
        }

        public class Request
        {
            public Image image { get; set; }
            public List<Feature> features { get; set; }
        }

        public class RootObject_GoogleTextDetect2
        {
            public List<Request> requests { get; set; }
        }



        /// <summary>
        /// post request for Gooogle translate api async.
        /// </summary>
        /// <returns>the translated text by google async.</returns>
        /// <param name="GoogleTranslate">Item to add.</param>
        public async Task<string> GoogleTranslateAsync(GoogleTranSource transsource)
        {
            var data = JsonConvert.SerializeObject(transsource);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            //think about the KEY.
            // Execute the REST API call.
            var response = await client.PostAsync("https://translation.googleapis.com/language/translate/v2?key=AIzaSyBf3aybUgE0aEvKgFRnBhZVN09V3S-A2js", content);

            // Get the JSON response.
            string contentString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RootObject_Translate>(JsonPrettyPrint(contentString));
            string final_test = "";
            var obj_data = obj.data;
            var obj_trans = obj_data.translations;
            foreach (var trtext in obj_trans)

            {
                final_test += trtext.translatedText;
            }

            return final_test;


        }


        public class GoogleTranSource
        {

            public string q { get; set; }
            public string target { get; set; }
        }





        public class Translation
        {
            public string translatedText { get; set; }
            public string detectedSourceLanguage { get; set; }
        }

        public class Data
        {
            public List<Translation> translations { get; set; }
        }

        public class RootObject_Translate
        {
            public Data data { get; set; }
        }


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


    }
}

