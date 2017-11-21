using FactOrFictionCommon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FactOrFictionTextHandling.MLClient
{
    public class LuisClient : IMLClient<LuisResult>
    {
        public string BaseUri { get; set; }

        public LuisClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        public async Task<LuisResult> Query(string sentenceFragment)
        {
            var uri = new Uri(this.BaseUri + HttpUtility.UrlEncode(sentenceFragment));
            var request = WebRequest.Create(uri);
            var response = await request.GetResponseAsync();
            return await ReadAllAsync(response.GetResponseStream());
        }

        internal static async Task<LuisResult> ReadAllAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<LuisResult>(await reader.ReadToEndAsync());
            }
        }
    }

    public class IntentObj
    {
        public string Intent { get; set; }
        public float Score { get; set; }
    }

    public class EntityObj
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }

    public class LuisResult : IMLResult
    {
        public string Query { get; set; }
        public IntentObj TopScoringIntent { get; set; }
        public List<IntentObj> Intents { get; set; }
        public List<EntityObj> Entities { get; set; }
        public SentenceType GetSentenceType()
        {
            var type = SentenceType.OTHER;
            try
            {
                switch (TopScoringIntent.Intent)
                {
                    case "SuggestedFact":
                    case "SuggestedQuantitativeFact":
                        type = SentenceType.OBJECTIVE;
                        break;
                    case "SuggestedOpinion":
                        type = SentenceType.SUBJECTIVE;
                        break;
                    default:
                        type = SentenceType.OTHER;
                        break;
                }
            }
            catch (ArgumentException)
            {
                type = SentenceType.OTHER;
            }
            return type;
        }
    }
}
