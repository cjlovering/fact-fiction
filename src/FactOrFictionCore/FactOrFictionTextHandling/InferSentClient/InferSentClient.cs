using FactOrFictionCommon.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.InferSentClient
{
    public class InferSentClient
    {
        public string BaseUri { get; set; }
        public string ApiKey { get; set; }

        public InferSentClient(String baseUri, string apiKey)
        {
            BaseUri = baseUri;
            ApiKey = apiKey;
        }

        public async Task<Sentence[]> ConnectInferSent(Sentence[] sentences) 
        {
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
                    _sentences = sentences
                }))
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);
            var responseContent = response.Content.ReadAsStringAsync()
                .Result
                .Trim('"')
                .Replace(@"\""", @"""")
                .Replace(@"\\", @"\");

            InferSentResponseObj content
                = JsonConvert.DeserializeObject<InferSentResponseObj>(responseContent);

            for (int i = 0; i < content.Sentences.Length; i++)
            {
                sentences[i].InferSentVectorsString = content.Vectors[i];
            }
            return sentences;
        }
    }

    public class InferSentResponseObj
    {
        [JsonProperty("sentences")]
        public string[] Sentences { get; set; }
        [JsonProperty("vectors")]
        public string[] Vectors { get; set; }
    }
}
