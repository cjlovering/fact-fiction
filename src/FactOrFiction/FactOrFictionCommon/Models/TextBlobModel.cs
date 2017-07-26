using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using FactOrFictionCommon.Validators;
using Newtonsoft.Json;

namespace FactOrFictionCommon.Models
{
    public class TextBlobModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        public string Text { get; set; }
        public string CreatedBy { get; set; }
        public List<Statement> Statements { get; set; }

        public TextBlobModel() { }

        public TextBlobModel(TextBlobModel textBlob)
        {
            this.Id = textBlob.Id;
            this.Text = textBlob.Text;
            this.Statements = textBlob.Statements.OrderBy(x => x.IndexInParent).ToList();
        }
    }

    public enum StatementClassification
    {
        Other,
        SuggestedFact,
        SuggestedQuantitativeFact,
        SuggestedOpinion
    }

    public class Statement
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Classification is required.")]
        public StatementClassification Classification { get; set; }
        public int IndexInParent { get; set; }
        public List<Reference> References { get; set; }
        public Guid? TextBlobModelId { get; set; }

        public Statement() { }

        public Statement(Statement statement, List<Reference> references)
        {
            this.Id = statement.Id;
            this.Text = statement.Text;
            this.Classification = statement.Classification;
            this.IndexInParent = statement.IndexInParent;
            this.References = references;
            this.TextBlobModelId = statement.TextBlobModelId;
        }
    }

    public class Reference
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }

        [Column("Tags")]
        public string TagsString { get; set; }

        [NotMapped]
        public List<string> Tags {
            get { return JsonConvert.DeserializeObject<List<string>>(this.TagsString); }
            set { this.TagsString = JsonConvert.SerializeObject(value); } }

        [Column("Link")]
        [Required(ErrorMessage = "Link is required.")]
        [UriValidator]
        public string LinkString { get; set; }

        [NotMapped]
        public Uri Link
        {
            get { return new Uri(this.LinkString); }
            set { this.LinkString = value.AbsoluteUri; }
        }
        public Bias Bias { get; set; }
        public Guid? StatementId { get; set; }
        public Guid? BiasId { get; set; }
    }
}