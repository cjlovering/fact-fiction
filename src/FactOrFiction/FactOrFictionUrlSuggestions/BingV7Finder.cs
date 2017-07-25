using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactOrFictionUrlSuggestions
{
    public sealed class BingV7Finder : CognitiveServicesFinder
    {
        protected override string Endpoint { get; } = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        protected override string SubscriptionKey { get; } = "d0ae93fad8c249fda81005fb56d79b3d";

        protected override IReadOnlyList<Uri> ParseJson(JObject json)
        {
            return json["webPages"]["value"]
                .Select(obj => new Uri(obj["url"].ToString()))
                .ToList();
        }
    }
}
