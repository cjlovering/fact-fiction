using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactOrFictionWeb.Models
{
    public class TextBlobModel
    {
        public Guid Id { get; set; }
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
        public Guid Id { get; set; }
        public StatementClassification Classification { get; set; }
        public List<Reference> References { get; set; } 
    }

    public class Reference
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public int Rating { get; set; }
        public string Link { get; set; }
    }
}