using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
using FactOrFictionCommon.Models;

namespace FactOrFictionUrlSuggestions
{
    public sealed class EntityFinder : CognitiveServicesFinder
    {
        protected override string Endpoint { get; } = "https://westus.api.cognitive.microsoft.com/entitylinking/v1.0/link";
        protected override string SubscriptionKey { get; } = "a2d4e8071a1f48dd93ea5986ed265a88";
        public async Task<JToken> GetEntities(string query)
        {
            var response = await QueryCognitiveServices(query);
            var json = JObject.Parse(response);
            return GetEntitiesFromJson(json);
        }
        protected override IReadOnlyList<Uri> ParseJson(JObject json)
        {
            return GetEntitiesFromJson(json)
                .Select(ExtractEntityWikiUrl)
                .ToList();
        }
        public string ExtractEntityName(JToken token)
        {
            return token["name"].ToString();
        }
        public string ExtractEntityWikiUrlString(JToken token)
        {
            var wikiId = token["wikipediaId"].ToString();
            return "https://en.wikipedia.org/wiki/" + HttpUtility.UrlPathEncode(wikiId);
        }
        public Uri ExtractEntityWikiUrl(JToken token)
        {
            return new Uri(ExtractEntityWikiUrlString(token));
        }
        private JToken GetEntitiesFromJson(JObject json)
        {
            return json["entities"];
        }
        protected override Dictionary<string, string> GetParams(string query)
        {
            return new Dictionary<string, string>();
        }
        protected override WebRequest GetWebRequest(string query)
        {
            var webRequest = base.GetWebRequest(query);
            SetRequestBody(webRequest, query);
            return webRequest;
        }
        private void SetRequestBody(WebRequest webRequest, string body)
        {
            webRequest.Method = "POST";
            webRequest.ContentType = "text/plain";
            var data = Encoding.UTF8.GetBytes(body);
            webRequest.ContentLength = data.Length;
            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }
        internal Persona GetPolitifactPersona(JToken entity)
        {
            var name = ExtractEntityName(entity);
            return PersonasDBLookups.ByName[name].FirstOrDefault();
        }
        public List<Tuple<string, int>> ExtractMatches(JToken token)
        {
            return token["matches"]
                .SelectMany(m =>
                    m["entries"].Select(e => Tuple.Create(m["text"].ToString(), int.Parse(e["offset"].ToString()))))
                .ToList();
        }
    }
}
