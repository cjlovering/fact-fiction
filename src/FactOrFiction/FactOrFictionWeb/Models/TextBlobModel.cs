using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FactOrFictionWeb.Models
{
    public class TextBlobModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        public string Text { get; set; }
        public List<Statement> Statements { get; set; } 
    }

    public enum StatementClassification
    {
        Other,
        SuggestedFact,
        SuggestedQuantitativeFact,
    }

    public class Statement
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Classification is required.")]
        public StatementClassification Classification { get; set; }
        public List<Reference> References { get; set; } 
    }

    public class Reference
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Rating is required.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Link is required.")]
        public string Link { get; set; }
    }
}