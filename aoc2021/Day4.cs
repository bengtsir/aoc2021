using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day4Board
    {
        public int[][] Numbers;

        public bool Bingo()
        {
            if (Numbers.Any(x => x.Sum() == 0))
            {
                return true;
            }

            for (int i = 0; i < 5; i++)
            {
                if (Numbers.Sum(x => x[i]) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Mark(int number)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Numbers[i][j] == number)
                    {
                        Numbers[i][j] = 0;
                        return Bingo();
                    }
                }
            }

            return false;
        }
    }

    internal class Day4
    {
        private List<int> moves = new List<int>();

        private List<Day4Board> boards = new List<Day4Board>();

        internal void ParseBoards()
        {
            var data = File.ReadAllLines(@"data\day4.txt");

            moves = data[0].Split(',').Select(x => Int32.Parse(x)).ToList();

            var sep = new char[] { ' ' };
            var opt = StringSplitOptions.RemoveEmptyEntries;

            int i = 2;

            while (i + 5 < data.Length)
            {
                var b = new Day4Board();

                b.Numbers = new[]
                {
                    data[i].Split(sep, opt).Select(x => Int32.Parse(x)).ToArray(),
                    data[i + 1].Split(sep, opt).Select(x => Int32.Parse(x)).ToArray(),
                    data[i + 2].Split(sep, opt).Select(x => Int32.Parse(x)).ToArray(),
                    data[i + 3].Split(sep, opt).Select(x => Int32.Parse(x)).ToArray(),
                    data[i + 4].Split(sep, opt).Select(x => Int32.Parse(x)).ToArray(),
                };

                boards.Add(b);

                i += 6;
            }
        }

        public void Part1()
        {
            ParseBoards();

            foreach (var move in moves)
            {
                foreach (var board in boards)
                {
                    if (board.Mark(move))
                    {
                        var sum = board.Numbers.Sum(x => x.Sum());

                        Console.WriteLine($"Move: {move}, Boardsum: {sum}, product: {move * sum}");
                        return;
                    }
                }
            }
        }

        public void Part2()
        {
            ParseBoards();

            foreach (var move in moves)
            {
                foreach (var board in boards)
                {
                    if (board.Mark(move))
                    {
                        var sum = board.Numbers.Sum(x => x.Sum());

                        Console.WriteLine($"Move: {move}, Boardsum: {sum}, product: {move * sum}");
                    }
                }

                boards.RemoveAll(b => b.Bingo());

                if (boards.Count == 0)
                {
                    return;
                }
            }
        }
    }
}