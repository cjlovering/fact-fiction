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
    public abstract class BingFinder : IFinder
    {
        protected abstract string BingUrl { get; }
        protected abstract string CognitiveServicesSubscriptionKey { get; }

        public async Task<IReadOnlyList<Uri>> FindSuggestions(string query)
        {
            var response = await QueryBing(query);
            var json = JObject.Parse(response);
            return ParseJson(json);
        }

        protected abstract IReadOnlyList<Uri> ParseJson(JObject json);

        protected virtual async Task<string> QueryBing(string query)
        {
            var httpParams = new Dictionary<string, string>
            {
                //["mkt"] = "en-US",
                //["responseFilter"] = "Webpages,News",
                ["q"] = query
            };
            var webRequest = WebRequest.Create(BingUrl + "?" + UrlEncode(httpParams));
            webRequest.Method = "GET";
            webRequest.Headers["Ocp-Apim-Subscription-Key"] = CognitiveServicesSubscriptionKey;
            var webResponse = await webRequest.GetResponseAsync();
            return await ReadAllAsync(webResponse.GetResponseStream());
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
