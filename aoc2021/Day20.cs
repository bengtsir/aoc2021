using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day20
    {
        int ShiftIn(int x, int bit)
        {
            return (x << 1) | bit;
        }

        int[][] Iterate(int[][] input, int[] key)
        {
            var output = input.Select(r => r.Select(x => (int)0).ToArray()).ToArray();

            for (int r = 1; r < input.Length - 1; r++)
            {
                for (int k = 1; k < input[0].Length - 1; k++)
                {
                    int bits = 0;
                    bits = ShiftIn(bits, input[r - 1][k - 1]);
                    bits = ShiftIn(bits, input[r - 1][k    ]);
                    bits = ShiftIn(bits, input[r - 1][k + 1]);
                    bits = ShiftIn(bits, input[r    ][k - 1]);
                    bits = ShiftIn(bits, input[r    ][k    ]);
                    bits = ShiftIn(bits, input[r    ][k + 1]);
                    bits = ShiftIn(bits, input[r + 1][k - 1]);
                    bits = ShiftIn(bits, input[r + 1][k    ]);
                    bits = ShiftIn(bits, input[r + 1][k + 1]);

                    output[r][k] = key[bits];
                }
            }

            return output;
        }

        void PrintGrid(int[][] grid)
        {
            foreach (var line in grid)
            {
                Console.WriteLine(new string(line.Select(i => i == 0 ? '.' : '#').ToArray()));
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day20.txt");

            var key = data[0].Select(c => c == '#' ? 1 : 0).ToArray();

            var input = data.Skip(2).Select(row => row.Select(x => x == '#'? 1 : 0).ToList()).ToList();
            var blanks = data[2].Select(x => 0).ToArray();
            var tenBlanks = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (var i = 0; i < 10; i++)
            {
                input.Insert(0, blanks.ToList());
                input.Add(blanks.ToList());
            }

            foreach (var line in input)
            {
                line.InsertRange(0, tenBlanks);
                line.AddRange(tenBlanks);
            }

            var grid = input.Select(l => l.ToArray()).ToArray();

            PrintGrid(grid);

            grid = Iterate(grid, key);
            PrintGrid(grid);
            grid = Iterate(grid, key);
            PrintGrid(grid);

            var ones = grid.Skip(2).Take(grid.Length - 4).Sum(r => r.Skip(2).Take(grid[0].Length - 4).Sum());

            Console.WriteLine($"Number of ones: {ones}");
        }

        public void Part2()
        {
            var iterations = 50;

            int guard = 2 * iterations + 10;

            var data = File.ReadAllLines(@"data\day20.txt");

            var key = data[0].Select(c => c == '#' ? 1 : 0).ToArray();

            var input = data.Skip(2).Select(row => row.Select(x => x == '#' ? 1 : 0).ToList()).ToList();
            var blanks = data[2].Select(x => 0).ToArray();
            var guardBlanks = Enumerable.Range(0, guard).Select(x => 0).ToArray();

            for (var i = 0; i < guard; i++)
            {
                input.Insert(0, blanks.ToList());
                input.Add(blanks.ToList());
            }

            foreach (var line in input)
            {
                line.InsertRange(0, guardBlanks);
                line.AddRange(guardBlanks);
            }

            var grid = input.Select(l => l.ToArray()).ToArray();

            for (int i = 0; i < iterations; i++)
            {
                grid = Iterate(grid, key);
            }
            
            var ones = grid.Skip(iterations).Take(grid.Length - 2*iterations).Sum(r => r.Skip(iterations).Take(grid[0].Length - 2*iterations).Sum());

            Console.WriteLine($"Number of ones: {ones}");
        }

    }
}
