using FactOrFictionCommon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.SentenceProducer
{
    public interface ISentenceProducer
    {
        Task<List<Task<Sentence>>> GetStatements(TextEntry textEntry);
    }
}
