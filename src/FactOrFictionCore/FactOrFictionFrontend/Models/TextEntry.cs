using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactOrFictionFrontend.Models
{
    public class TextEntry
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ICollection<Sentence> Sentences { get; set; }
    }
}


