using FactOrFictionCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public interface IFinder
    {
        /// <summary>
        /// For a given string query, returns a list of suggested URLs.
        /// </summary>
        Task<IReadOnlyList<Uri>> FindSuggestions(string query);
    }
    
    public interface URLClassifier
    {
        
        /// <summary>
        /// For a given string query, representing a news outlet (eg. nytimes), returns a classification
        /// </summary>
        /// Note: description should actually be uri.host
        Task<Bias> ClassifyOutletDescription(string description);
    }
}
