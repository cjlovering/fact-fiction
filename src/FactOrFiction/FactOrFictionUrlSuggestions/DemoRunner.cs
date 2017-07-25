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
                var classificationTasks = urls
                    .Select(url => classifier.ClassifyOutletDescription(ExtractDomain(url)))
                    .ToArray();
                var classifications = await Task.WhenAll(classificationTasks);
                for (int i = 0; i < urls.Count; i++)
                {
                    Console.WriteLine($"[{classifications[i]}] ({ExtractDomain(urls[i])}) {urls[i]}");
                }
            }
            catch (WebException e)
            {
                var errorMessage = await BingFinder.ReadAllAsync(e.Response.GetResponseStream());
                Console.WriteLine(errorMessage);
            }
        }

        private static string ExtractDomain(Uri uri)
        {
            var splits = uri.Authority.Split('.');
            return splits.Length >= 2
                ? splits.Reverse().Skip(1).Take(1).First()
                : uri.Authority;
        }

        private static string ReadLine()
        {
            Console.Write("Enter query: ");
            return Console.ReadLine();
        }
    }
}
