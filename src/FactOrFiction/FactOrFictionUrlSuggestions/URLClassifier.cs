using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionUrlSuggestions
{
    public sealed class URLClassification : URLClassifier
    {
        
        public Task<string> ClassifyOutletDescription(string description)
        {
            Bias bias = BiasDBLookups.ByHostName[description].FirstOrDefault();
            return Task.FromResult(bias?.BiasType);
        }
     
    }

    
}