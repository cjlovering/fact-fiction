using System;
using System.Linq;
using FactOrFictionCommon.Models;

namespace FactOrFictionUrlSuggestions
{
    public static class PersonasDBLookups
    {
        public static ILookup<string, FactOrFictionCommon.Models.Persona> ByName = PersonasDB.Personas.ToLookup(
            keySelector: p => p.Name,
            comparer: StringComparer.OrdinalIgnoreCase);
    }
}
