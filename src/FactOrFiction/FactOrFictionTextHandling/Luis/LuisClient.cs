using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FactOrFictionTextHandling.Luis
{
    public class LuisClient : ILuisClient
    {
        public WebClient Client { get; private set; }
        public string BaseUri { get; set; }

        public LuisClient(LuisWebClient client, string baseUri)
        {
            Client = client;
            BaseUri = baseUri;
        }

        public string Query(string sentenceFragment)
        {
            var uri = new Uri(this.BaseUri + HttpUtility.UrlEncode(sentenceFragment));
            return Client.DownloadString(uri);
        }
    }
}
