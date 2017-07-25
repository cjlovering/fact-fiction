using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.Luis
{
    public interface ILuisClientFactory
    {
        ILuisClient Create();
    }
}
