using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public static class PersonasDBLookups
    {
        public static ILookup<string, Persona> ByName = PersonasDB.Personas.ToLookup(
            keySelector: p => p.Name,
            comparer: StringComparer.OrdinalIgnoreCase);
    }
}
