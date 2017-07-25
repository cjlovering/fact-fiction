using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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

        [Column("Tags")]
        [Required(ErrorMessage = "Tags is required.")]
        public string TagsString { get; set; }

        [NotMapped]
        public List<string> Tags {
            get { return JsonConvert.DeserializeObject<List<string>>(this.TagsString); }
            set { this.TagsString = JsonConvert.SerializeObject(value); } }

        [Column("Link")]
        [Required(ErrorMessage = "Link is required.")]
        public string LinkString { get; set; }

        [NotMapped]
        public Uri Link
        {
            get { return new Uri(this.LinkString); }
            set { this.LinkString = value.AbsoluteUri; }
        }
    }
}