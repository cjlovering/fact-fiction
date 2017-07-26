using System;
using System.Diagnostics.Contracts;

namespace FactOrFictionCommon.Models
{
    public sealed class Bias
    {

        public string Source { get; set; }
        public string Factuality { get; set; }
        public string BiasType { get; set; }
        public string Notes { get; set; }
        public Guid Id { get; set; }
        public Guid? ReferenceId { get; set; }

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

        public Bias(Bias bias, Guid id)
        {
            this.Source = bias.Source;
            this.Factuality = bias.Factuality;
            this.BiasType = bias.BiasType;
            this.Notes = bias.Notes;
            this.Id = id;
        }

        public Bias()
        {

        }

        private void Requires(bool v)
        {
            if (!v)
            {
                throw new ArgumentException();
            }
        }
        //Instead, use this: awk -F "\"*,\"*" '{print "new Bias(source: \""$1"\", factuality: \""$2"\", biasType: \""$3"\", notes: \"" $4 "\"),"}' index.csv
        public string DisplayString
        {
            get
            {
                return $"Bias Type: {BiasType}<BR/>\n" +
                    $"Factuality: {Factuality}<BR/>\n" +
                    $"Notes: {Notes}<BR/>";
            }
        }
    }
}