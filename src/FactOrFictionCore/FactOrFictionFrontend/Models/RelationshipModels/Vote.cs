using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactOrFictionFrontend.Models.RelationshipModels
{
    public class Vote
    {
        public DateTime Timestamp { get; set; }
        public VoteType Type { get; set; }

        public string UserId { get; set; }
        public int SentenceId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual Sentence Sentence { get; set; }
    }

    public enum VoteType
    {
        TRUE,
        FALSE
    }
}
