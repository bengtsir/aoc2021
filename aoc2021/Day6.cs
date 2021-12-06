using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day6
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            var fish = new int[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0,
            };

            foreach (var idx in data[0].Split(',').Select(x => Int32.Parse(x)))
            {
                fish[idx]++;
            }

            Console.WriteLine($"Initial fish: {fish.Sum()}");

            for (int i = 0; i < 80; i++)
            {
                var zeros = fish[0];
                fish = fish.Skip(1).Append(zeros).ToArray();
                fish[6] += zeros;

                Console.WriteLine($"Day {i + 1}: {fish.Sum()}");
            }
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            var fish = new long[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0,
            };

            foreach (var idx in data[0].Split(',').Select(x => Int32.Parse(x)))
            {
                fish[idx]++;
            }

            Console.WriteLine($"Initial fish: {fish.Sum()}");

            for (int i = 0; i < 256; i++)
            {
                var zeros = fish[0];
                fish = fish.Skip(1).Append(zeros).ToArray();
                fish[6] += zeros;

                Console.WriteLine($"Day {i + 1}: {fish.Sum()}");
            }
        }
    }
}
