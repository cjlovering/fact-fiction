using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FactOrFictionTextHandling.Luis
{
    public class LuisClient : ILuisClient
    {
        public string BaseUri { get; set; }

        public LuisClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        public async Task<LuisResult> Query(string sentenceFragment)
        {
            var uri = new Uri(this.BaseUri + HttpUtility.UrlEncode(sentenceFragment));
            var request = WebRequest.Create(uri);
            var response = await request.GetResponseAsync();
            return await ReadAllAsync(response.GetResponseStream());
        }

        internal static async Task<LuisResult> ReadAllAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<LuisResult>(await reader.ReadToEndAsync());
            }
        }
    }
}
