using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.Luis
{
    public class LuisClientFactory : ILuisClientFactory
    {
        public LuisWebClient Client { get; set; }
        public string BaseUri { get; set; }

        public LuisClientFactory(LuisWebClient client, string baseUri)
        {
            Client = client;
            BaseUri = baseUri;
        }

        public ILuisClient Create()
        {
            return new LuisClient(this.Client, this.BaseUri);
        }
    }
}
