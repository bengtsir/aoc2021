using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Number
    {
        public char[] Rep { get; }
        public int N { get; set; }

        public Number(string data)
        {
            Rep = data.Select(c => c).ToArray();
            N = -1; 
        }
    }

    internal class Day8
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            var segments = data.Select(x => x.Split('|').Select(y => y.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray()).ToList();

            var sum = new int[] { 2, 4, 3, 7 }.Sum(x => segments.Sum(s => s[1].Count(y => y.Length == x)));

            Console.WriteLine($"Number of 1, 4, 7, 8: {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            var segments = data.Select(x => x.Split('|').Select(y => y.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray()).ToList();

            long totSum = 0;

            foreach (var segment in segments)
            {
                var rawNumbers = segment[0].Select(s => new Number(s)).ToArray();
                var sortedNumbers = new Number[10];

                // Known numbers

                sortedNumbers[1] = rawNumbers.First(n => n.Rep.Length == 2);
                sortedNumbers[4] = rawNumbers.First(n => n.Rep.Length == 4);
                sortedNumbers[7] = rawNumbers.First(n => n.Rep.Length == 3);
                sortedNumbers[8] = rawNumbers.First(n => n.Rep.Length == 7);

                rawNumbers = rawNumbers.Where(n => n.Rep.Length != 2 && n.Rep.Length != 4 && n.Rep.Length != 3 && n.Rep.Length != 7).ToArray();

                sortedNumbers[9] = rawNumbers.First(n =>
                    n.Rep.Length == 6 && n.Rep.Except(sortedNumbers[7].Rep.Union(sortedNumbers[4].Rep)).Count() == 1);
                rawNumbers = rawNumbers.Where(n => n.Rep != sortedNumbers[9].Rep).ToArray();

                sortedNumbers[6] = rawNumbers.First(n =>
                    n.Rep.Length == 6 && sortedNumbers[8].Rep.Except(n.Rep).Intersect(sortedNumbers[1].Rep).Count() == 1);
                rawNumbers = rawNumbers.Where(n => n.Rep != sortedNumbers[6].Rep).ToArray();

                sortedNumbers[0] = rawNumbers.First(n => n.Rep.Length == 6);
                rawNumbers = rawNumbers.Where(n => n.Rep != sortedNumbers[0].Rep).ToArray();

                sortedNumbers[3] = rawNumbers.First(n => n.Rep.Intersect(sortedNumbers[1].Rep).Count() == 2);
                rawNumbers = rawNumbers.Where(n => n.Rep != sortedNumbers[3].Rep).ToArray();

                sortedNumbers[5] = rawNumbers.First(n => sortedNumbers[6].Rep.Except(n.Rep).Count() == 1);
                rawNumbers = rawNumbers.Where(n => n.Rep != sortedNumbers[5].Rep).ToArray();

                // There should only be 1 left
                sortedNumbers[2] = rawNumbers[0];

                for (int i = 0; i < 10; i++)
                {
                    sortedNumbers[i].N = i;
                }

                var sum = 0;
                foreach (var digit in segment[1])
                {
                    sum = sum * 10 + sortedNumbers.First(n => n.Rep.OrderBy(x => x).SequenceEqual(digit.Select(x => x).ToList().OrderBy(x => x))).N;
                }
                Console.WriteLine($"Local sum: {sum}");

                totSum += sum;
            }
            Console.WriteLine($"Total sum: {totSum}");

        }

    }
}
