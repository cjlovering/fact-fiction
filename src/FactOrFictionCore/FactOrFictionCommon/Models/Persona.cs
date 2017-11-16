using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactOrFictionCommon.Models
{
    public sealed class Persona
    {
        private const string PolitifactPeopleEndpoint = "http://www.politifact.com/api/statements/truth-o-meter/people/";

        public readonly string Href;
        public readonly string Name;
        public readonly string Party;
        public readonly string NameSlug;
        private StatementByPersona[] m_statements;

        public Persona(string name, string href, string party)
        {
            Requires(name != null);
            Requires(href != null);

            Name = name;
            NameSlug = href.Replace("personalities/", "").Replace("/", "");
            Href = href;
            Party = party;
            m_statements = null;
        }

        private StatementByPersona[] GetStatements()
        {
            return m_statements ?? FetchRecentStatements().GetAwaiter().GetResult();
        }

        public async Task<StatementByPersona[]> FetchRecentStatements()
        {
            var webRequest = WebRequest.Create(PolitifactPeopleEndpoint + NameSlug + "/json/?n=15");
            webRequest.Method = "GET";
            var webResponse = await webRequest.GetResponseAsync();
            var response = await ReadAllAsync(webResponse.GetResponseStream());
            var arr = JArray.Parse(response);
            m_statements = arr
                .Select(a => new StatementByPersona
                {
                    Ruling = a["ruling"]["ruling"].ToString(),
                    RulingSlug = a["ruling"]["ruling_slug"].ToString(),
                    StatementHtml = a["statement"].ToString()
                })
                .ToArray();
            return m_statements;
        }

        private static async Task<string> ReadAllAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
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

        public string PolitifactUrl => GetFullUrl();

        public string PolitifactScoreAsHtml()
        {
            var scores = GetStatements()
                .GroupBy(s => s.Ruling)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => $"<span class=\"{ToSlug(grp.Key)}\">{grp.Key}</span><span class=\"{ToSlug(grp.Key)}_count\"> ({grp.Count()})</span>");
            return string.Join("<span>, </span>", scores);
        }

        private static string ToSlug(string key)
        {
            return key.Replace(" ", "").Replace("!", "");
        }

        public string GetFullUrl()
        {
            return "http://www.politifact.com" + Href;
        }
    }

    public class Reference
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }

        public string TagsString { get; set; }

        public List<string> Tags
        {
            get { return JsonConvert.DeserializeObject<List<string>>(this.TagsString); }
            set { this.TagsString = JsonConvert.SerializeObject(value); }
        }

        public string LinkString { get; set; }

        public Uri Link
        {
            get { return new Uri(this.LinkString); }
            set { this.LinkString = value.AbsoluteUri; }
        }
        public Bias Bias { get; set; }
        public Guid? StatementId { get; set; }
        public Guid? BiasId { get; set; }
    }

    public class Entity
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string WikiUrl { get; set; }
        public List<Match> Matches { get; set; }
        [NotMapped]
        public Persona Persona { get; set; }
        [NotMapped]
        public string PolitifactUrl => Persona?.PolitifactUrl;
        [NotMapped]
        public string PolitifactScoreAsHtml => Persona?.PolitifactScoreAsHtml();
        [NotMapped]
        public bool HasPersona => Persona != null;
    }

    public class Match
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Offset { get; set; }
        public Guid? EntityId { get; set; }
    }

    public class StatementByPersona
    {
        public string Ruling { get; set; }
        public string RulingSlug { get; set; }
        public string StatementHtml { get; set; }
    }
}
