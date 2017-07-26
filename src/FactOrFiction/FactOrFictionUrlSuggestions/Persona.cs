using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using static FactOrFictionUrlSuggestions.CognitiveServicesFinder;

namespace FactOrFictionUrlSuggestions
{
    public sealed class Persona
    {
        private const string PolitifactPeopleEndpoint = "http://www.politifact.com/api/statements/truth-o-meter/people/";

        public readonly string Href;
        public readonly string Name;
        public readonly string Party;
        public readonly string NameSlug;

        public Persona(string name, string href, string party)
        {
            Requires(name != null);
            Requires(href != null);

            Name = name;
            NameSlug = href.Replace("personalities/", "").Replace("/", "");
            Href = href;
            Party = party;
        }

        public async Task<StatementByPersona[]> GetRecentStatements()
        {
            var webRequest = WebRequest.Create(PolitifactPeopleEndpoint + NameSlug + "/json/?n=15");
            webRequest.Method = "GET";
            var webResponse = await webRequest.GetResponseAsync();
            var response = await ReadAllAsync(webResponse.GetResponseStream());
            var arr = JArray.Parse(response);
            return arr
                .Select(a => new StatementByPersona
                {
                    Ruling = a["ruling"]["ruling"].ToString(),
                    StatementHtml = a["statement"].ToString()
                })
                .ToArray();
        }

        private void Requires(bool v)
        {
            if (!v)
            {
                throw new ArgumentException();
            }
        }

        public string ToEvalString()
        {
            var partyStr = Party != null
                ? '"' + Party + '"'
                : "null";
            return $"new Persona(name: \"{Name}\", href: \"{Href}\", party: {partyStr})";
        }

        internal string GetFullUrl()
        {
            return "http://www.politifact.com" + Href;
        }
    }

    public class StatementByPersona
    {
        public string Ruling { get; set; }
        public string StatementHtml { get; set; }
    }
}
