using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Pair18
    {
        public int LeftVal { get; set; }
        public int RightVal { get; set; }

        public Pair18 LeftPair { get; set; }
        public Pair18 RightPair { get; set; }

        public Pair18()
        {
            // Nothing
        }

        public Pair18(string s)
        {
            Parse(s);
        }

        public override string ToString()
        {
            var s = "[";

            if (LeftPair != null)
            {
                s += LeftPair.ToString();
            }
            else
            {
                s += LeftVal.ToString();
            }

            s += ",";

            if (RightPair != null)
            {
                s += RightPair.ToString();
            }
            else
            {
                s += RightVal.ToString();
            }

            s += "]";

            return s;
        }

        public string Parse(string s)
        {
            if (s[0] != '[')
            {
                throw new Exception("No opening bracket");
            }

            if (s[1] == '[')
            {
                LeftPair = new Pair18();
                s = LeftPair.Parse(s.Substring(1));
            }
            else
            {
                LeftVal = s[1] - '0';
                s = s.Substring(2);
            }

            if (s[0] != ',')
            {
                throw new Exception("Comma missing");
            }

            if (s[1] == '[')
            {
                RightPair = new Pair18();
                s = RightPair.Parse(s.Substring(1));
            }
            else
            {
                RightVal = s[1] - '0';
                s = s.Substring(2);
            }

            if (s[0] != ']')
            {
                throw new Exception("Closing bracket missing");
            }

            s = s.Substring(1);

            return s;
        }

        public int Magnitude()
        {
            var l = LeftPair != null ? LeftPair.Magnitude() : LeftVal;
            var r = RightPair != null ? RightPair.Magnitude() : RightVal;

            return 3 * l + 2 * r;
        }

        public Pair18 Add(Pair18 other)
        {
            var p = new Pair18
            {
                LeftPair = this,
                RightPair = other
            };

            p.Reduce();

            return p;
        }

        public void Reduce()
        {
            //Console.WriteLine(ToString());
            while (Explode(4) != null || Split())
            {
                //Console.WriteLine(ToString());
            }
        }

        internal Pair18 Explode(int depth, int rval = -1, int lval = -1)
        {
            if (rval >= 0)
            {
                // Search for a place for it

                if (LeftPair == null)
                {
                    LeftVal += rval;
                    return null;
                }
                else
                {
                    return LeftPair.Explode(depth - 1, rval);
                }
            }

            if (lval >= 0)
            {
                // Search

                if (RightPair == null)
                {
                    RightVal += lval;
                    return null;
                }
                else
                {
                    return RightPair.Explode(depth - 1, -1, lval);
                }
            }

            if (depth == 0)
            {
                return this;
            }

            if (LeftPair == null && RightPair == null)
            {
                return null;
            }

            if (LeftPair != null)
            {
                var p = LeftPair.Explode(depth - 1);

                if (p != null && depth == 1)
                {
                    LeftPair = null;
                    LeftVal = 0;
                }

                if (p != null && RightPair == null && p.RightVal >= 0)
                {
                    RightVal += p.RightVal;
                    p.RightVal = -1;
                }

                if (p != null && RightPair != null && p.RightVal >= 0)
                {
                    RightPair.Explode(depth - 1, p.RightVal);
                    p.RightVal = -1;
                }

                if (p != null)
                {
                    return p;
                }
            }

            if (RightPair != null)
            {
                var p = RightPair.Explode(depth - 1);

                if (p != null && depth == 1)
                {
                    RightPair = null;
                    RightVal = 0;
                }

                if (p != null && LeftPair == null && p.LeftVal >= 0)
                {
                    LeftVal += p.LeftVal;
                    p.LeftVal = -1;
                }

                if (p != null && LeftPair != null && p.LeftVal >= 0)
                {
                    LeftPair.Explode(depth - 1, -1, p.LeftVal);
                    p.LeftVal = -1;
                }

                if (p != null)
                {
                    return p;
                }
            }

            return null;
        }

        internal bool Split()
        {
            if (LeftPair == null && LeftVal >= 10)
            {
                var p = new Pair18
                {
                    LeftVal = LeftVal / 2,
                    RightVal = (LeftVal + 1) / 2
                };

                LeftPair = p;

                return true;
            }

            if (LeftPair != null && LeftPair.Split())
            {
                return true;
            }

            if (RightPair == null && RightVal >= 10)
            {
                var p = new Pair18()
                {
                    LeftVal = RightVal / 2,
                    RightVal = (RightVal + 1) / 2
                };

                RightPair = p;

                return true;
            }

            if (RightPair != null && RightPair.Split())
            {
                return true;

            }

            return false;
        }
    }

    internal class Day18
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            var p1 = new Pair18();
            p1.Parse(@"[[[[4,3],4],4],[7,[[8,4],9]]]");

            var p2 = new Pair18();
            p2.Parse(@"[1,1]");

            var p3 = p1.Add(p2);

            Console.WriteLine($"{p3}");
            
            var testData = new[]
            {
                "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
                "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
                "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                "[7,[5,[[3,8],[1,4]]]]",
                "[[2,[2,2]],[8,[8,1]]]",
                "[2,9]",
                "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                "[[[5,[7,4]],7],1]",
                "[[[[4,2],2],6],[8,7]]",
            };

            var sum = testData.Skip(1).Aggregate(new Pair18(testData.First()), (agg, s) => agg.Add(new Pair18(s)));

            Console.WriteLine($"{sum}");

            var pp = new Pair18("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
            Console.WriteLine($"{pp.Magnitude()}");
            */

            var sum = data.Skip(1).Aggregate(new Pair18(data.First()), (agg, s) => agg.Add(new Pair18(s)));

            Console.WriteLine($"Final sum is {sum}");
            Console.WriteLine($"Magnitude is {sum.Magnitude()}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            data = new string[]
            {
                "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
                "[[[5,[2,8]],4],[5,[[9,9],0]]]",
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
                "[[[[5,4],[7,7]],8],[[8,3],8]]",
                "[[9,3],[[9,9],[6,[4,9]]]]",
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
            };
            */

            Pair18 largestPair = new Pair18("[0,0]");
            int largestMagnitude = -1;

            var pairs = data.Select(s => new Pair18(s)).ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    if (i != j)
                    {
                        var s = new Pair18(data[i]).Add(new Pair18(data[j]));// pairs[i].Add(pairs[j]);

                        if (s.Magnitude() > largestMagnitude)
                        {
                            largestMagnitude = s.Magnitude();
                            largestPair = s;
                        }
                    }
                }
            }

            Console.WriteLine($"Largest poly is {largestPair}");
            Console.WriteLine($"Magnitude is {largestPair.Magnitude()}");
        }

    }
}