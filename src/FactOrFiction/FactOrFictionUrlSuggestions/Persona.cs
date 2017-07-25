using System;
using System.Diagnostics.Contracts;

namespace FactOrFictionUrlSuggestions
{
    public sealed class Persona
    {
        private const string Separator = "\t";

        public readonly string Href;
        public readonly string Name;
        public readonly string Party;

        public Persona(string name, string href, string party)
        {
            Requires(name != null);
            Requires(href != null);
            Requires(!name.Contains(Separator));
            Requires(!href.Contains(Separator));
            Requires(party == null || !party.Contains(Separator));

            Name = name;
            Href = href;
            Party = party;
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
}
