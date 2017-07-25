using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FactOrFictionUrlSuggestions
{
    public abstract class CognitiveServicesFinder : IFinder
    {
        protected abstract string Endpoint { get; }
        protected abstract string SubscriptionKey { get; }

        public async Task<IReadOnlyList<Uri>> FindSuggestions(string query)
        {
            var response = await QueryCognitiveServices(query);
            var json = JObject.Parse(response);
            return ParseJson(json);
        }

        protected abstract IReadOnlyList<Uri> ParseJson(JObject json);

        protected virtual async Task<string> QueryCognitiveServices(string query)
        {
            var webRequest = GetWebRequest(query);
            var webResponse = await webRequest.GetResponseAsync();
            return await ReadAllAsync(webResponse.GetResponseStream());
        }

        protected virtual WebRequest GetWebRequest(string query)
        {
            var httpParams = GetParams(query);
            var webRequest = WebRequest.Create(Endpoint + "?" + UrlEncode(httpParams));
            webRequest.Method = "GET";
            webRequest.Headers["Ocp-Apim-Subscription-Key"] = SubscriptionKey;
            return webRequest;
        }

        protected virtual Dictionary<string, string> GetParams(string query)
        {
            return new Dictionary<string, string>
            {
                ["q"] = query
            };
        }

        internal static async Task<string> ReadAllAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        internal static string UrlEncode(Dictionary<string, string> httpParams)
        {
            return string.Join(
                "&",
                httpParams.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
        }
    }
}
