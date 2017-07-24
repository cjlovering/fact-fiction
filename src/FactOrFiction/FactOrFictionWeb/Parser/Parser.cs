using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace FactOrFictionWeb.Parser
{
    public class ShittyParser
    {
        private static string[] getWords(string input)
        {
            var start = 0;
            var index = 0;
            var tupleList = new List<Tuple<int, int>>();

            while (index < input.Length)
            {
                char curr = input[index];
                char prev = input[Math.Max(0, index - 1)];
                char next = input[Math.Min(input.Length - 1, index + 1)];

                if (!Char.IsLetter(curr) || !Char.IsNumber(curr))
                {
                    if ((curr == ',' || curr == '.') && Char.IsNumber(prev) && Char.IsNumber(next)) // if curr is a comma separating number
                    {
                        continue;
                    }

                    tupleList.Add(new Tuple<int, int>(start, index));
                    start = index + 1;
                }
                index++;
            }

            return tupleList.Select(x => input.Substring(x.Item1, x.Item2 - x.Item1)).ToArray();
        }
        

        public static string[] puctuationParse(string input, char delimiter)
        {
            var start = 0;
            var index = 0;
            var tupleList = new List<Tuple<int, int>>();

            while (index < input.Length)
            {
                char ch = input[index];
                char prev = input[Math.Max(0, index - 1)];
                char next = input[Math.Min(input.Length - 1, index + 1)];
                char nextnext = input[Math.Min(input.Length - 1, index + 2)];

                if (ch == '.' || ch == 8212 || ch == 63 || ch == 33 || ch == 8211) // . 
                {
                    if (ch == '.' 
                        && ((Char.IsNumber(prev) && Char.IsNumber(next)) 
                                || (Char.IsUpper(prev) && (Char.IsUpper(next) || next == ',' || Char.IsLower(nextnext))))) // if curr is a period separating number
                    {
                        index++;
                        continue;
                    }
                    start = index + 1;
                }
                index++;
            }
            string[] tokens = tupleList.Select(x => input.Substring(x.Item1, x.Item2 - x.Item1))
                .Select(x => x)
                .ToArray();
            return tokens;
        }

        public static string[] quoteParse(string input, char delimiter)
        {
            var quoteTokens = new HashSet<char> { '\"' };
            var quoteStack = new Stack<char>();
            var tupleList = new List<Tuple<int, int>>();
            var start = 0;
            int index = 0;
            while (index < input.Length)
            {
                char ch = input[index];
                if (quoteStack.Count == 0 && ch == delimiter)
                {
                    tupleList.Add(new Tuple<int, int>(start, index));
                    start = index + 1;
                }
                else if (quoteTokens.Contains(ch))
                {
                    if (quoteStack.Count != 0 && quoteStack.Peek() == ch)
                    {
                        quoteStack.Pop();
                    }
                    else
                    {
                        quoteStack.Push(ch);
                    }
                }
                index += 1;
            }
            if (start <= input.Length)
            {
                tupleList.Add(new Tuple<int, int>(start, input.Length));
            }
            return tupleList.Select(x => input.Substring(x.Item1, x.Item2 - x.Item1)).ToArray();
        }
    }
}