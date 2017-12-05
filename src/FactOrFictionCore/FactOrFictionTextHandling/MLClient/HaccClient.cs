using FactOrFictionCommon.Models;
using FactOrFictionTextHandling.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FactOrFictionTextHandling.MLClient
{
    public class HaccClient : IParser, IMLClient<HaccResult>
    {

        private Dictionary<int, HaccResult> _classification = new Dictionary<int, HaccResult>();

        public string BaseUri { get; set; }
        public string ApiKey { get; set; }

        public HaccClient(string baseUri, string apiKey)
        {
            BaseUri = baseUri;
            ApiKey = apiKey;
        }

        public async Task<Dictionary<int, string>> Parse(string textEntry)
        {
            Dictionary<int, string> parsedSentences = new Dictionary<int, string>();

            textEntry = textEntry.Trim();
            if (String.IsNullOrEmpty(textEntry))
            {
                return parsedSentences;
            }

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(BaseUri)
            };
            
            //For local web service, comment out this line.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    text_entry = textEntry
                }))
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseContent = response.Content.ReadAsStringAsync()
                .Result
                .Trim('"')
                .Replace(@"\""", @"""")
                .Replace(@"\\", @"\");

            HaccResponseObj content
                = JsonConvert.DeserializeObject<HaccResponseObj>(responseContent);
            int position = 0;
            for (int i = 0; i < content.Sentences.Length; i++)
            {
                parsedSentences.Add(position, content.Sentences[i]);
                _classification.Add(position, new HaccResult
                {
                    Prediction = content.Prediction[i]
                });
                position += content.Sentences[i].Length + 1;
            }
            return parsedSentences;
        }

        public Task<HaccResult> Query(KeyValuePair<int, string> sentenceWithPosition)
        {
            return Task.FromResult(_classification[sentenceWithPosition.Key]);
        }

    }

    public class HaccResponseObj
    {
        [JsonProperty("sentences")]
        public string[] Sentences { get; set; }
        [JsonProperty("pred")]
        public string[] Prediction { get; set; }
    }

    public class HaccResult : IMLResult
    {
        public string Prediction { get; set; }
        public SentenceType GetSentenceType()
        {
            switch (Prediction)
            {
                case "objective":
                    return SentenceType.OBJECTIVE;
                case "subjective":
                    return SentenceType.SUBJECTIVE;
                default:
                    return SentenceType.OTHER;
            }
        }
    }
}
