using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ying.DataService
{
   public class QueryResouces
    {
        HttpClient client = new HttpClient();

        /// <summary>
        /// Gets the resource items async.
        /// </summary>
        /// <returns>The resource items async.</returns>
          public async Task<List<ResourceItem>> GetResourceItemsAsync()
        {

            var response = await client.GetStringAsync("http://18.220.1.200/api/ying_urls/QueryAll");
            var ResourceItem = JsonConvert.DeserializeObject<List<ResourceItem>>(response);
            return ResourceItem;
        }


        /// <summary>
        /// Gets the resource items by description key words async.
        /// </summary>
        /// <returns>The resource items async.</returns>
        public async Task<List<ResourceItem>> GetResourceItemByDescsAsync(string desc)
        {
            string url = "http://localhost:60761/api/ying_urls/Querybydesc?desc=" + desc;


            var response = await client.GetStringAsync(url);
            var ResourceItem = JsonConvert.DeserializeObject<List<ResourceItem>>(response);
            return ResourceItem;
        }


        public class ResourceItem
        {
            public int id { get; set; }
            public string type { get; set; }
            public string url { get; set; }
            public string description { get; set; }
            public DateTime entrytime { get; set; }
            public string valid_flag { get; set; }

            public string icon { get; set; }

        }



    }
}
