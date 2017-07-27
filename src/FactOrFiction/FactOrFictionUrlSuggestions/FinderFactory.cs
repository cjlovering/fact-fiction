using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public static class FinderFactory
    {
        public static IFinder CreateFinder()
        {
            return new BingV5Finder();
        }
    }
}
