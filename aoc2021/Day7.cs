using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day7
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            var positions = data[0].Split(',').Select(x => Int32.Parse(x)).ToArray();

            long fuel = 999999999999999;
            int minpos = 0;

            for (var pos = positions.Min(); pos <= positions.Max(); pos++)
            {
                long f = positions.Sum(x => Math.Abs(x - pos));
                if (f < fuel)
                {
                    fuel = f;
                    minpos = pos;
                }
            }

            Console.WriteLine($"Min fuel: {fuel} at position {minpos}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            var positions = data[0].Split(',').Select(x => Int32.Parse(x)).ToArray();

            long fuel = 999999999999999;
            int minpos = 0;

            for (var pos = positions.Min(); pos <= positions.Max(); pos++)
            {
                long f = positions.Sum(x => (Math.Abs(x - pos)*(Math.Abs(x - pos) + 1))/2);
                if (f < fuel)
                {
                    fuel = f;
                    minpos = pos;
                }
            }

            Console.WriteLine($"Min fuel: {fuel} at position {minpos}");
        }
    }
}
