using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.Parser
{
    public interface IParser
    {
        Task<Dictionary<int, string>> Parse(string input);
    }
}
