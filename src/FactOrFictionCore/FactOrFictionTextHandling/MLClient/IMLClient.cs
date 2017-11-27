using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.MLClient
{
    public interface IMLClient<T> where T: IMLResult
    {
        Task<T> Query(KeyValuePair<int, string> sentenceWithPosition);
    }
}
