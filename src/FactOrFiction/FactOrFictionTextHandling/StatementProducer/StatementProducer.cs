using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.UI;
using FactOrFictionTextHandling.Luis;
using FactOrFictionTextHandling.Parser;
using FactOrFictionWeb.Models;

namespace FactOrFictionTextHandling.StatementProducer
{
    public class StatementProducer
    {
        public ILuisClient LuisClient { get; set; }
        public StatementProducer(ILuisClient luisClient)
        {
            LuisClient = luisClient;
        }

        public List<Task<Statement>> GetStatements(TextBlobModel textBlob)
        {
            var statementTasks = ShittyParser.PuctuationParse(textBlob.Text).Select
                (async text => new Statement
                    {
                        Id = Guid.NewGuid(),
                        Text = text,
                        Classification = await GetStatementClassification(text),
                        References = null
                    }
                );

            return statementTasks.ToList();
        }

        public async Task<StatementClassification> GetStatementClassification(String text)
        {
            LuisResult response = await LuisClient.Query(text);
            StatementClassification classification = StatementClassification.Other;
            try
            {
                classification =
                    (StatementClassification)
                        Enum.Parse(typeof (StatementClassification), response.TopScoringIntent.Intent, ignoreCase: true);
            }
            catch (ArgumentException)
            {
                classification = StatementClassification.Other;
            }
            return classification;
        }
    }
}
