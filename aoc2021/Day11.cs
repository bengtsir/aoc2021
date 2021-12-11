using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day11
    {
        public long Flash(ref int[][] grid)
        {
            long flashes = 0;

            for (int y = 1; y < 11; y++)
            {
                for (int x = 1; x < 11; x++)
                {
                    grid[y][x]++;
                }
            }

            bool change = true;

            while (change)
            {
                change = false;
                for (int y = 1; y < 11; y++)
                {
                    for (int x = 1; x < 11; x++)
                    {
                        if (grid[y][x] > 9)
                        {
                            change = true;
                            grid[y][x] = 0;
                            flashes++;

                            for (int yo = y - 1; yo <= y + 1; yo++)
                            {
                                for (int xo = x - 1; xo <= x + 1; xo++)
                                {
                                    if (grid[yo][xo] > 0)
                                    {
                                        grid[yo][xo]++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return flashes;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day11.txt");

            var input = data.Select(row => row.Select(x => (int)(x - '0')).ToList()).ToList();
            var nines = data[0].Select(x => 9).ToArray();

            input.Insert(0, nines.ToList());
            input.Add(nines.ToList());

            foreach (var line in input)
            {
                line.Insert(0, 9);
                line.Add(9);
            }

            var grid = input.Select(l => l.ToArray()).ToArray();

            long sum = 0;

            for (int i = 0; i < 100; i++)
            {
                sum += Flash(ref grid);
            }

            Console.WriteLine($"Number of flashes: {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day11.txt");

            var input = data.Select(row => row.Select(x => (int)(x - '0')).ToList()).ToList();
            var nines = data[0].Select(x => 9).ToArray();

            input.Insert(0, nines.ToList());
            input.Add(nines.ToList());

            foreach (var line in input)
            {
                line.Insert(0, 9);
                line.Add(9);
            }

            var grid = input.Select(l => l.ToArray()).ToArray();

            long sum = 0;
            int step = 0;

            while (sum < 100)
            {
                step++;
                sum = Flash(ref grid);
                Console.WriteLine($"Number of flashes: {sum} at step {step}");
            }

            Console.WriteLine($"Number of flashes: {sum} at step {step}");
        }

    }
}
