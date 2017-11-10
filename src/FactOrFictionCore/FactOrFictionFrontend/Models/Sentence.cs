using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactOrFictionFrontend.Models
{
    public class Sentence
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int VoteTrue { get; set; }
        public int VoteFalse { get; set; }
        public float Confidence { get; set; }
        public SentenceType Type { get; set; }

        public Guid TextEntryId { get; set; }
        public virtual TextEntry OriginalTextEntry { get; set; }

    }

    public enum SentenceType
    {
        SUBJECTIVE,
        OBJECTIVE,
        OTHER
    }
}
