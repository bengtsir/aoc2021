using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day5
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            var grid = new int[1000,1000];

            var sep = new char[] { ' ', ',', '-', '>' };
            var opt = StringSplitOptions.RemoveEmptyEntries;

            foreach (var line in data)
            {
                var n = line.Split(sep, opt).Select(x => Int32.Parse(x)).ToArray();

                if (n[0] == n[2])
                {
                    for (int i = Math.Min(n[1], n[3]); i <= Math.Max(n[1],n[3]); i++)
                    {
                        grid[n[0], i]++;
                    }
                }
                else if (n[1] == n[3])
                {
                    for (int i = Math.Min(n[0], n[2]); i <= Math.Max(n[0], n[2]); i++)
                    {
                        grid[i, n[1]]++;
                    }
                }
            }

            var count = 0;

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    if (grid[i,j] > 1)
                    {
                        {
                            count++;
                        }
                    }
                }
            }

            Console.WriteLine($"Number of overlaps: {count}");
        }


        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            var grid = new int[1000, 1000];

            var sep = new char[] { ' ', ',', '-', '>' };
            var opt = StringSplitOptions.RemoveEmptyEntries;

            foreach (var line in data)
            {
                var n = line.Split(sep, opt).Select(x => Int32.Parse(x)).ToArray();

                if (n[0] == n[2])
                {
                    for (int i = Math.Min(n[1], n[3]); i <= Math.Max(n[1], n[3]); i++)
                    {
                        grid[n[0], i]++;
                    }
                }
                else if (n[1] == n[3])
                {
                    for (int i = Math.Min(n[0], n[2]); i <= Math.Max(n[0], n[2]); i++)
                    {
                        grid[i, n[1]]++;
                    }
                }
                else
                {
                    var xstep = n[2] > n[0] ? 1 : -1;
                    var ystep = n[3] > n[1] ? 1 : -1;
                    var ofs = 0;

                    while (n[0] + ofs*xstep != n[2])
                    {
                        grid[n[0] + ofs * xstep, n[1] + ofs * ystep]++;
                        ofs++;
                    }
                    grid[n[2], n[3]]++;
                }
            }

            var count = 0;

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    if (grid[i, j] > 1)
                    {
                        {
                            count++;
                        }
                    }
                }
            }

            Console.WriteLine($"Number of overlaps: {count}");
        }
    }
}
