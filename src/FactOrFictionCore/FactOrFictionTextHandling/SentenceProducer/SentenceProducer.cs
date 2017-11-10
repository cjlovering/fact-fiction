using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactOrFictionTextHandling.Luis;
using FactOrFictionTextHandling.Parser;
using FactOrFictionCommon.Models;

namespace FactOrFictionTextHandling.SentenceProducer
{
    public class SentenceProducer
    {
        public ILuisClient LuisClient { get; set; }
        public SentenceProducer(ILuisClient luisClient)
        {
            LuisClient = luisClient;
        }

        public List<Task<Sentence>> GetStatements(TextEntry textEntry)
        {
            var statementTasks = WorkingParser.PunctuationParse(textEntry.Content).Select
                (async text => new Sentence
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
                );

            return statementTasks.ToList();
        }

        public async Task<SentenceType> GetStatementClassification(String text)
        {
            LuisResult response = await LuisClient.Query(text);
            SentenceType classification = SentenceType.OTHER;
            try
            {
                switch (response.TopScoringIntent.Intent)
                {
                    case "SuggestedFact":
                    case "SuggestedQuantitativeFact":
                        classification = SentenceType.OBJECTIVE;
                        break;
                    case "SuggestedOpinion":
                        classification = SentenceType.SUBJECTIVE;
                        break;
                    default:
                        classification = SentenceType.OTHER;
                        break;
                }
            }
            catch (ArgumentException)
            {
                classification = SentenceType.OTHER;
            }
            return classification;
        }
    }
}
