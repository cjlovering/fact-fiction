namespace FactOrFictionTextHandling.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ShittyParser
    {
        private static string[] GetWords(string input)
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
        public static string[] PuctuationParse(string input)
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
            string[] array = tupleList.Select(x => input.Substring(x.Item1, x.Item2 - x.Item1))
                .ToArray();
            LinkedList<string> nonEmpty = new LinkedList<string>();
            foreach (string x in array) {
                if (!x.Trim().Equals(""))
                {
                    nonEmpty.AddLast(x.Trim());
                }
                array = nonEmpty.ToArray();
            }
            return array;
        }

        public static string[] QuoteParse(string input, char delimiter)
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