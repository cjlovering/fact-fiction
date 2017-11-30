using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionCommon.Models
{
    public class Sentence : IComparable
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        [DisplayName("Vote True")]
        public int VoteTrue { get; set; }
        [DisplayName("Vote False")]
        public int VoteFalse { get; set; }
        public int Position { get; set; }
        public float Confidence { get; set; }
        public SentenceType Type { get; set; }

        public Guid TextEntryId { get; set; }
        public virtual TextEntry OriginalTextEntry { get; set; }
        public string InferSentVectorsString { get; set; }
        
        [NotMapped]
        public double[] InferSentVectorsDouble
        {
            get
            {
                return Array.ConvertAll(InferSentVectorsString.Split(';'), Double.Parse);
            }
            set
            {
                InferSentVectorsString = String.Join(";", value.Select(p => p.ToString()).ToArray());
            }
        }

        public int CompareTo(object obj)
        {
            Sentence other = (Sentence)obj;
            return Position.CompareTo(other.Position);
        }
    }

    public enum SentenceType
    {
        SUBJECTIVE,
        OBJECTIVE,
        OTHER
    }
}
