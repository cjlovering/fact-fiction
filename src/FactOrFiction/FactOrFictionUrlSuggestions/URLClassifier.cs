using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public sealed class URLClassification : URLClassifier
    {
        
        public Task<string> ClassifyOutletDescription(string description)
        {
            var baseURL = "https://mediabiasfactcheck.com/?s=";
            var anchorTag = "//div[@class='mh-excerpt']";

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(baseURL + description);

            var newsSources = doc.DocumentNode.SelectNodes(anchorTag).ToList();

            var classification = "None";

            if (newsSources.Count != 0)
            {
                string possibleClassification = newsSources[0].InnerText;
                classification = ParseClassification(possibleClassification);
            }

            return Task.FromResult(classification);
        }

        private static string ParseClassification(string desc)
        {
            var classification = "";
            foreach (string word in desc.Split(' '))
            {
                if (!IsAllUpper(word))
                {
                    break;
                }
                classification += word  + " ";
            }

            return classification.Trim();
        }

        private static bool IsAllUpper(string input)
        {
            return input.All(ch => !char.IsLetter(ch) || char.IsUpper(ch));
        }
    }

    
}