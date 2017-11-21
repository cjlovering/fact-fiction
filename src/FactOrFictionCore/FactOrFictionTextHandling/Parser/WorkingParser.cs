using System.Security.Cryptography.X509Certificates;

namespace FactOrFictionTextHandling.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WorkingParser : IParser
    {
        public Task<Dictionary<int, string>> Parse(string input)
        {
            var start = 0;
            var index = 0;
            var tupleList = new List<Tuple<int, int>>();

            while (index < input.Length)
            {
                char ch = input[index];
                char prev = input[Math.Max(0, index - 1)];
                char prevprev = input[Math.Max(0, index - 2)];
                char next = input[Math.Min(input.Length - 1, index + 1)];
                char nextnext = input[Math.Min(input.Length - 1, index + 2)];

                if (ch == '.' || ch == 8212 || ch == 63 || ch == 33 || ch == 8211 || next == '\n') 
                {
                    if (ch == '.'
                        && ((Char.IsNumber(prev) && Char.IsNumber(next)) // if curr is a period or comma separated number, U.S. in the middle or sentence
                                || (Char.IsUpper(prev) && (Char.IsUpper(next) || next == ',' || Char.IsLower(nextnext))))) 
                    {
                        index++;
                        continue;
                    }

                    if (ch == '.' && next == 34) // next is end quote
                    {
                        tupleList.Add(new Tuple<int, int>(start, index + 2));
                        start = index + 2;

                        index += 2;
                        continue;
                    }

                    // honorifics or titles or whatever
                    if (ch == '.' 
                        && ((prevprev == 'M' && prev == 'r') // please just dont demo articles that have "Mrs." in them
                          ||(prevprev == 'M' && prev == 's')
                          ||(prevprev == 'J' && prev == 'r')
                          ||(prevprev == 'S' && prev == 'r')
                          ||(prevprev == 'D' && prev == 'r')))
                    {
                        index++;
                        continue;
                    } 

                    tupleList.Add(new Tuple<int, int>(start, index + 1));
                    start = index + 1;
                }
                index++;
                if (index == input.Length && start != index + 1 && start != index)
                {
                    tupleList.Add(new Tuple<int, int>(start, index));
                }
            }

            var result = tupleList
                .Select(x => new KeyValuePair<int, string>(x.Item1, input.Substring(x.Item1, x.Item2 - x.Item1).Trim()))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .ToDictionary(x => x.Key, x => x.Value);

            return Task.FromResult(result);
        }
    }
}