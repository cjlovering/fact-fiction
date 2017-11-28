using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactOrFictionTextHandling.MLClient;
using FactOrFictionTextHandling.Parser;
using FactOrFictionCommon.Models;

namespace FactOrFictionTextHandling.SentenceProducer
{
    public class SentenceProducer<T> : ISentenceProducer where T: IMLResult
    {
        public IMLClient<T> MLClient { get; set; }
        public IParser Parser { get; set; }

        public SentenceProducer(IMLClient<T> mlClient, IParser parser)
        {
            MLClient = mlClient;
            Parser = parser;
        }

        public async Task<List<Task<Sentence>>> GetStatements(TextEntry textEntry)
        {
            var sentences = await Parser.Parse(textEntry.Content);
            var classification = sentences.Select(async sentenceWithPosition => new Sentence
            {
                Id = Guid.NewGuid(),
                Content = sentenceWithPosition.Value,
                Position = sentenceWithPosition.Key,
                Type = await GetStatementClassification(sentenceWithPosition),
                TextEntryId = textEntry.Id,
                VoteFalse = 0,
                VoteTrue = 0,
                //Confidence = 0
            }
            );
            return classification.ToList();
        }

        public async Task<SentenceType> GetStatementClassification(KeyValuePair<int, string> sentenceWithPosition)
        {
            IMLResult response = await MLClient.Query(sentenceWithPosition);
            return response.GetSentenceType();
        }
    }
}
