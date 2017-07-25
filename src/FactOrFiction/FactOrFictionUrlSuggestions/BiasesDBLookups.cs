using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public static class BiasDBLookups
    {
        public static ILookup<string, Bias> ByHostName = BiasDB.Biases.ToLookup(
            keySelector: getHost,
            comparer: StringComparer.OrdinalIgnoreCase);

        private static String getHost(Bias b)
        {
            try
            {
                return new Uri(b.Source).Host;
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}
