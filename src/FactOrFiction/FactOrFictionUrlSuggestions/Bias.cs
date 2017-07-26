using System;
using System.Diagnostics.Contracts;

namespace FactOrFictionUrlSuggestions
{
    public sealed class Bias
    {

        public readonly string Source;
        public readonly string Factuality;
        public readonly string BiasType;
        public readonly string Notes;

        public Bias(string source, string factuality, string biasType, string notes)
        {
            Requires(source != null);
            Requires(factuality != null);
            Requires(biasType != null);
            Requires(notes != null);

            Source = source;
            Factuality = factuality;
            BiasType = biasType;
            Notes = notes;
        }

        private void Requires(bool v)
        {
            if (!v)
            {
                throw new ArgumentException();
            }
        }
        //Instead, use this: awk -F "\"*,\"*" '{print "new Bias(source: \""$1"\", factuality: \""$2"\", biasType: \""$3"\", notes: \"" $4 "\"),"}' index.csv
        public string ToEvalString()
        {
            return $"new Bias(source: {Source}, factuality: {Factuality}, biasType: {BiasType}), notes: {Notes})";
        }
    }
}