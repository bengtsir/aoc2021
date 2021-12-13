using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day13
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            var board = new int[2000,2000];

            var pointRe = new Regex(@"^(?<x>\d+),(?<y>\d+)$");
            var foldRe = new Regex(@"^fold along (?<dir>[xy])=(?<pos>\d+)$");

            foreach (var m in data.Select(s => pointRe.Match(s)).Where(m => m.Success))
            {
                board[Int32.Parse(m.Groups["x"].Value), Int32.Parse(m.Groups["y"].Value)] = 1;
            }

            var f = data.Select(s => foldRe.Match(s)).First(m => m.Success);

            if (f.Groups["dir"].Value == "x")
            {
                var xFold = Int32.Parse(f.Groups["pos"].Value);

                for (int y = 0; y < 2000; y++)
                {
                    for (int x = 1; xFold - x >= 0; x++)
                    {
                        board[xFold - x, y] += board[xFold + x, y];
                        board[xFold + x, y] = 0;
                    }
                }
            }
            else
            {
                var yFold = Int32.Parse(f.Groups["pos"].Value);

                for (int x = 0; x < 2000; x++)
                {
                    for (int y = 1; yFold - y >= 0; y++)
                    {
                        board[x, yFold - y] += board[x, yFold + y];
                        board[x, yFold + y] = 0;
                    }
                }
            }

            // Count

            var count = 0;

            for (int x = 0; x < 2000; x++)
            {
                for (int y = 0; y < 2000; y++)
                {
                    if (board[x,y] > 0)
                    {
                        count++;
                    }
                }
            }

            Console.WriteLine($"Count is {count}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            var board = new int[2000, 2000];

            var pointRe = new Regex(@"^(?<x>\d+),(?<y>\d+)$");
            var foldRe = new Regex(@"^fold along (?<dir>[xy])=(?<pos>\d+)$");

            foreach (var m in data.Select(s => pointRe.Match(s)).Where(m => m.Success))
            {
                board[Int32.Parse(m.Groups["x"].Value), Int32.Parse(m.Groups["y"].Value)] = 1;
            }

            foreach (var f in data.Select(s => foldRe.Match(s)).Where(m => m.Success))
            {
                if (f.Groups["dir"].Value == "x")
                {
                    var xFold = Int32.Parse(f.Groups["pos"].Value);

                    for (int y = 0; y < 2000; y++)
                    {
                        for (int x = 1; xFold - x >= 0; x++)
                        {
                            board[xFold - x, y] += board[xFold + x, y];
                            board[xFold + x, y] = 0;
                        }
                    }
                }
                else
                {
                    var yFold = Int32.Parse(f.Groups["pos"].Value);

                    for (int x = 0; x < 2000; x++)
                    {
                        for (int y = 1; yFold - y >= 0; y++)
                        {
                            board[x, yFold - y] += board[x, yFold + y];
                            board[x, yFold + y] = 0;
                        }
                    }
                }

            }

            // Print

            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write(board[x, y] > 0 ? "X": " ");
                }
                Console.WriteLine();
            }

        }

    }
}
