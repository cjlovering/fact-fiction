using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactOrFictionTextHandling.Luis
{
    public class IntentObj
    {
        public string Intent { get; set; }
        public float Score { get; set; }
    }

    public class EntityObj
    {
        public string Entity { get; set; }
        public string Type { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }



    public class LuisResult
    {
        public string Query { get; set; }
        public IntentObj TopScoringIntent { get; set; }
        public List<IntentObj> Intents { get; set; }
        public List<EntityObj> Entities { get; set; }
    }

}
