using FactOrFictionCommon.Models;

namespace FactOrFictionTextHandling.MLClient
{
    public interface IMLResult
    {
        SentenceType GetSentenceType();
    }
}
