using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day9
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day9.txt");

            var input = data.Select(row => row.Select(x => (int)(x - '0')).ToList()).ToList();
            var nines = data[0].Select(x => 9).ToArray();

            input.Insert(0, nines.ToList());
            input.Add(nines.ToList());

            foreach (var line in input)
            {
                line.Insert(0, 9);
                line.Add(9);
            }

            long sum = 0;

            for (int y = 1; y < input.Count - 1; y++)
            {
                for (int x = 1; x < input[0].Count - 1; x++)
                {
                    if (input[y][x] < input[y][x - 1] &&
                        input[y][x] < input[y][x + 1] &&
                        input[y][x] < input[y - 1][x] &&
                        input[y][x] < input[y + 1][x])
                    {
                        sum += input[y][x] + 1;
                    }
                }
            }

            Console.WriteLine($"Sum of points: {sum}");
        }

        int CountBlob(List<List<int>> a, int x, int y)
        {
            if (a[y][x] == 9)
            {
                return 0;
            }

            a[y][x] = 9;
            return 1 + CountBlob(a, x-1, y) + CountBlob(a, x, y-1) + CountBlob(a, x+1, y) + CountBlob(a, x, y+1);
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day9.txt");

            var input = data.Select(row => row.Select(x => (int)(x - '0')).ToList()).ToList();
            var nines = data[0].Select(x => 9).ToArray();

            input.Insert(0, nines.ToList());
            input.Add(nines.ToList());

            foreach (var line in input)
            {
                line.Insert(0, 9);
                line.Add(9);
            }

            var blobSizes = new List<int>();

            for (int y = 1; y < input.Count - 1; y++)
            {
                for (int x = 1; x < input[0].Count - 1; x++)
                {
                    if (input[y][x] != 9)
                    {
                        blobSizes.Add(CountBlob(input, x, y));
                    }
                }
            }

            blobSizes.Sort();
            blobSizes.Reverse();

            long product = blobSizes[0] * blobSizes[1] * blobSizes[2];

            Console.WriteLine($"Sizes: {blobSizes[0]} {blobSizes[1]} {blobSizes[2]}, product: {product}");
        }
    }
}
