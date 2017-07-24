﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactOrFictionWeb.Parser
{
    public class Parser
    {
        public static string[] Parse(string input, char delimiter)
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