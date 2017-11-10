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
        public string BaseUri { get; set; }

        public LuisClientFactory(string baseUri)
        {
            BaseUri = baseUri;
        }

        public ILuisClient Create()
        {
            return new LuisClient(this.BaseUri);
        }
    }
}
