using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactOrFictionTextHandling.MLClient;
using FactOrFictionTextHandling.Parser;
using FactOrFictionCommon.Models;

namespace FactOrFictionTextHandling.SentenceProducer
{
    public class SentenceProducer<T> where T: IMLResult
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
            var parserTask = Parser.Parse(textEntry.Content);
            var getClassificationTask = parserTask.ContinueWith(parsedSentences =>
                parsedSentences.Result.Select(
                    async text => new Sentence
                    {
                        Id = Guid.NewGuid(),
                        Content = text.Value,
                        Position = text.Key,
                        Type = await GetStatementClassification(text.Value),
                        TextEntryId = textEntry.Id,
                        VoteFalse = 0,
                        VoteTrue = 0,
                        //Confidence = 0
                    }
                    ));
            return (await getClassificationTask).ToList();
        }

        public async Task<SentenceType> GetStatementClassification(String text)
        {
            IMLResult response = await MLClient.Query(text);
            return response.GetSentenceType();
        }
    }
}
