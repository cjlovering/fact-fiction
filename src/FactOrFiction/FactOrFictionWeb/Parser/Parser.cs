using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactOrFictionWeb.Parser
{
    public class Parser
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
                    tupleList.Add(new Tuple<int, int>(start, index));
                    start = index + 1;

                    if (curr == ',' && Char.IsNumber(prev) && Char.IsNumber(next)) // if curr is a comma separating number
                    {
                        continue;
                    }
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
                if (ch == '.' || ch == '-')
                {
                    tupleList.Add(new Tuple<int, int>(start, index));
                    start = index + 1;
                }
                index++;
            }
            return tupleList.Select(x => input.Substring(x.Item1, x.Item2 - x.Item1)).ToArray();
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