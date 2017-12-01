using System;

namespace FactOrFictionCommon.Models.SentenceViewModels
{
    public class SentenceViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int VoteTrue { get; set; }
        public int VoteFalse { get; set; }
        public SentenceViewModel(Sentence sent)
        {
            Id = sent.Id;
            Content = sent.Content;
            Type = sent.Type.ToString();
            VoteTrue = sent.VoteTrue;
            VoteFalse = sent.VoteFalse;
        }
    }
}
