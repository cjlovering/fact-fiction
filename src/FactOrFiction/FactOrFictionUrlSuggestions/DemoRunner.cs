using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    class DemoRunner
    {
        public static void Run(string[] args)
        {
            var classifier = new URLClassification();
            var finder = new BingV7Finder();
            var query = ReadLine();
            while (!string.IsNullOrEmpty(query))
            {
                ExecuteQuery(finder, classifier, query).GetAwaiter().GetResult();
                Console.WriteLine("=================================");
                query = ReadLine();
            }
        }

        private static async Task ExecuteQuery(IFinder finder, URLClassifier classifier, string query)
        {
            try
            {
                var urls = await finder.FindSuggestions(query);
                var entityFinder = new EntityFinder();
                Console.WriteLine("============ Web searches");
                var classificationTasks = urls
                    .Select(url => classifier.ClassifyOutletDescription(ExtractDomain(url)))
                    .ToArray();
                var classifications = await Task.WhenAll(classificationTasks);
                for (int i = 0; i < urls.Count; i++)
                {
                    Console.WriteLine($"  [{classifications[i]}] ({ExtractDomain(urls[i])}) {urls[i]}");
                }

                Console.WriteLine();
                Console.WriteLine("=========== Entities");
                var entities = await entityFinder.GetEntities(query);
                foreach (var entity in entities)
                {
                    var entityName = entityFinder.ExtractEntityName(entity);
                    var wikiUrl = entityFinder.ExtractEntityWikiUrl(entity);
                    var politifactPersona = entityFinder.GetPolitifactPersona(entity);
                    var politifactStr = politifactPersona != null
                        ? politifactPersona.GetFullUrl()
                        : "none found";
                    var party = politifactPersona != null
                        ? "(" + politifactPersona.Party + ")"
                        : "";
                    Console.WriteLine($"  '{entityName}' {party}");
                    Console.WriteLine($"    - wiki: {wikiUrl}");
                    Console.WriteLine($"    - politifact: {politifactStr}");
                    if (politifactPersona != null)
                    {
                        Console.WriteLine($"    - recent statements:");
                        var statements = await politifactPersona.GetRecentStatements();
                        foreach (var grp in statements.GroupBy(s => s.Ruling).OrderByDescending(grp => grp.Count()))
                        {
                            Console.WriteLine($"      - {grp.Key}: {grp.Count()}");
                        }
                    }
                }
            }
            catch (WebException e)
            {
                var errorMessage = e.Response != null
                    ? await CognitiveServicesFinder.ReadAllAsync(e.Response.GetResponseStream())
                    : e.Message;
                Console.WriteLine(errorMessage);
            }
        }

        private static string ExtractDomain(Uri uri)
        {
            return uri.Host;
        }

        private static string ReadLine()
        {
            Console.Write("Enter query: ");
            return Console.ReadLine();
        }
    }
}
