using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionCommon.Models
{
    public class Sentence : IComparable
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int VoteTrue { get; set; }
        public int VoteFalse { get; set; }
        public int Position { get; set; }
        public float Confidence { get; set; }
        public SentenceType Type { get; set; }

        public Guid TextEntryId { get; set; }
        public virtual TextEntry OriginalTextEntry { get; set; }

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
