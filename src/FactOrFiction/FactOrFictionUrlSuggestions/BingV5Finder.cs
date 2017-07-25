using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactOrFictionUrlSuggestions
{
    public sealed class BingV5Finder : CognitiveServicesFinder
    {
        protected override string Endpoint { get; } = "https://api.cognitive.microsoft.com/bing/v5.0/search";
        protected override string SubscriptionKey { get; } = "918ef5f1ab6040bbaf1d7d7965f5b5ad";

        protected override IReadOnlyList<Uri> ParseJson(JObject json)
        {
            return json["webPages"]["value"]
                .Select(obj =>
                {
                    // this url is in the form of "https://www.bing.com/cr?.....&r=<actual-url>&..."
                    var uri = new Uri(obj["url"].ToString());
                    // extract the "r" param from the returned url, which is the actual url of the result
                    var queryParams = HttpUtility.ParseQueryString(uri.Query);
                    return new Uri(queryParams["r"]);
                })
                .ToList();
        }
    }
}
