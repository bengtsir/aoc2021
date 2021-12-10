using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class IllegalCharException : Exception
    {
        public char C { get; set; }

        public IllegalCharException(char c)
        {
            C = c;
        }
    }

    internal class IncompleteException : Exception
    {
        public long Value { get; set; }

        public IncompleteException(long v)
        {
            Value = v;
        }
    }

    internal class Day10
    {
        public char Closing(char c)
        {
            switch (c)
            {
                case '(':
                    return ')';
                case '[':
                    return ']';
                case '<':
                    return '>';
                case '{':
                    return '}';
            }

            return 'x';
        }

        public string Parse(string s)
        {
            if (s.Length == 0)
            {
                return s;
            }

            var c = s[0];
            s = s.Substring(1);

            if (")]>}".Contains(c))
            {
                throw new IllegalCharException(c);
            }

            while (s.Length > 0 && "([<{".Contains(s[0]))
            {
                try
                {
                    s = Parse(s);
                }
                catch (IncompleteException e)
                {
                    throw new IncompleteException(e.Value * 5 + " ([{<".IndexOf(c));
                }
            }

            if (s.Length == 0)
            {
                throw new IncompleteException(" ([{<".IndexOf(c));
            }

            if (s[0] == Closing(c))
            {
                s = s.Substring(1);
            }
            else
            {
                throw new IllegalCharException(s[0]);
            }

            return s;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            long sum = 0;

            foreach (var line in data)
            {
                try
                {
                    Parse(line);
                }
                catch (IllegalCharException e)
                {
                    switch (e.C)
                    {
                        case ')':
                            sum += 3;
                            break;
                        case ']':
                            sum += 57;
                            break;
                        case '>':
                            sum += 25137;
                            break;
                        case '}':
                            sum += 1197;
                            break;
                    }
                }
                catch (IncompleteException)
                {
                    // Ignore for now
                }
            }

            Console.WriteLine($"Error sum is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            List<long> values = new List<long>();

            foreach (var line in data)
            {
                try
                {
                    Parse(line);
                }
                catch (IllegalCharException)
                {
                    // Do nothing
                }
                catch (IncompleteException e)
                {
                    values.Add(e.Value);
                }
            }

            values.Sort();

            Console.WriteLine($"Median value is {values[values.Count/2]}");
        }
    }
}
