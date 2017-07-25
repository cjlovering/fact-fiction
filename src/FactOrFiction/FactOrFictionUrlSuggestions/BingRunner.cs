using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    class BingRunner
    {
        public static void Run(string[] args)
        {
            var finder = new EntityFinder();
                //new BingV7Finder();
            var query = ReadLine();
            while (!string.IsNullOrEmpty(query))
            {
                ExecuteQuery(finder, query);
                Console.WriteLine("=================================");
                query = ReadLine();
            }
        }

        private static void ExecuteQuery(IFinder finder, string query)
        {
            try
            {
                var list = finder.FindSuggestions(query).GetAwaiter().GetResult();
                foreach (var url in list)
                {
                    Console.WriteLine(url);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(BingV5Finder.ReadAllAsync(e.Response.GetResponseStream()).GetAwaiter().GetResult());
            }
        }

        private static string ReadLine()
        {
            Console.Write("Enter query: ");
            return Console.ReadLine();
        }
    }
}
