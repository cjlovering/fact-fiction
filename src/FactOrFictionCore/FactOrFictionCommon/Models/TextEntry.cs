using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactOrFictionCommon.Models
{
    public class TextEntry
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        [DisplayName("Created at")]
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId"), DisplayName("Created by")]
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ICollection<Sentence> Sentences { get; set; }
    }
}


