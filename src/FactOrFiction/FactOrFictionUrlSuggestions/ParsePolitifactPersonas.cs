using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FactOrFictionCommon.Models;

namespace FactOrFictionUrlSuggestions
{
    class ParsePolitifactPersonas
    {
        private static Regex HrefRegex = new Regex("<a href=\"(?<href>[^\"]*)\"[^>]*>(?<name>.*)</a>");
        private static Regex PartyRegex = new Regex("<span class=\"people-party\">(?<party>.*)</span>");

        public static string ParseAndRender(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            var chunks = GetChunks(lines, 4);
            var personas = chunks.Select(ParseChunk);
            return string.Join(Environment.NewLine, personas.Select(p => p.ToEvalString() + ","));
        }

        private static Persona ParseChunk(string[] chunk)
        {
            var m = HrefRegex.Match(chunk[1]);
            var href = m.Groups["href"].Value.Trim();
            var name = m.Groups["name"].Value.Trim();
            var party = string.IsNullOrEmpty(chunk[2])
                ? null
                : PartyRegex.Match(chunk[2]).Groups["party"].Value.Trim();
            return new Persona(name, href, party);
        }

        private static List<string[]> GetChunks(string[] lines, int chunkSize)
        {
            var result = new List<string[]>();
            var offset = 0; 
            while (offset < lines.Length)
            {
                var chunk = new string[chunkSize];
                Array.Copy(lines, offset, chunk, 0, Math.Min(chunkSize, lines.Length - offset));
                result.Add(chunk);
                offset += chunkSize;
            }
            return result;
        }
    }
}
