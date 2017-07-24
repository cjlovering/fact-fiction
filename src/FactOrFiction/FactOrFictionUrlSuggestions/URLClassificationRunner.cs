using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    class URLClassificationRunner
    {
        public static void Run(string[] args)
        {
            var classifier = new URLClassification();
            var query = ReadLine();
            while (!string.IsNullOrEmpty(query))
            {
                ExecuteQuery(classifier, query);
                Console.WriteLine("=================================");
                query = ReadLine();
            }
        }

        private static void ExecuteQuery(URLClassifier classifier, string query)
        {
            try
            {
                var classification = classifier.ClassifyOutletDescription(query).GetAwaiter().GetResult();
                Console.WriteLine(classification);
            }
            catch (WebException e)
            {
                Console.WriteLine(BingFinder.ReadAllAsync(e.Response.GetResponseStream()).GetAwaiter().GetResult());
            }
        }

        private static string ReadLine()
        {
            Console.Write("Enter query: ");
            return Console.ReadLine();
        }
    }
}
