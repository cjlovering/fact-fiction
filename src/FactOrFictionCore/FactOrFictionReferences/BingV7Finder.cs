using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FactOrFictionUrlSuggestions
{
    public sealed class BingV7Finder : CognitiveServicesFinder
    {
        protected override string Endpoint { get; } = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        protected override string SubscriptionKey { get; }
        public BingV7Finder(string SubscriptionKey)
        {
            this.SubscriptionKey = SubscriptionKey;
        }
        protected override IReadOnlyList<Uri> ParseJson(JObject json)
        {
            if (json["webPages"] == null)
            {
                return new List<Uri>();
            }
            return json["webPages"]["value"]
                .Select(obj => new Uri(obj["url"].ToString()))
                .ToList();
        }
    }
}
