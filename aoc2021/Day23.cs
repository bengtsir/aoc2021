using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Board23
    {
        public int[] TopRow;

        public int[][] Bins;

        public List<int[]> Moves = new List<int[]>();

        public int Evaluation { get; set; }

        public bool[] TopOfBin = new[] { false, false, true, false, true, false, true, false, true, false, false };

        // Positions:
        // 0  1  2  3  4  5  6  7  8  9 10
        //     100   200   300   400
        //     101   201   301   401

        public Board23()
        {
            TopRow = new int[11];

            Bins = new[]
            {
                new[] { 1, 4 },
                new[] { 3, 1 },
                new[] { 2, 4 },
                new[] { 3, 2 }
            };
        }

        public void InitTestPattern()
        {
            Bins = new[]
            {
                new[] { 2, 1 },
                new[] { 3, 4 },
                new[] { 2, 3 },
                new[] { 4, 1 }
            };
        }

        public int Cost(int piece)
        {
            switch (piece)
            {
                case 1:
                    return 1;
                case 2:
                    return 10;
                case 3:
                    return 100;
                case 4:
                    return 1000;
            }

            return 0;
        }

        public bool Done()
        {
            for (int bin = 0; bin < 4; bin++)
            {
                if (Bins[bin][0] != (bin + 1) ||
                    Bins[bin][1] != (bin + 1))
                {
                    return false;
                }
            }

            return true;
        }

        public Board23(Board23 other)
        {
            TopRow = other.TopRow.Select(c => c).ToArray();
            Bins = other.Bins.Select(b => b.Select(v => v).ToArray()).ToArray();
            Evaluation = other.Evaluation;
        }

        public void GenerateMoves()
        {
            // Iterate over bins
            for (int bin = 3; bin >= 0; bin--)
            {
                // Top of bins
                if (TopRow[(bin*2) + 2] == 0 && Bins[bin][0] != 0 && (Bins[bin][1] != (bin + 1) || Bins[bin][0] != (bin + 1)))
                {
                    // Move it out
                    var topRowPos = (bin * 2) + 2;
                    var stepCost = Cost(Bins[bin][0]);
                    var offset = 1;

                    while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                    {
                        if (!TopOfBin[topRowPos - offset])
                        {
                            Moves.Add(new[] { (bin + 1) * 100, topRowPos - offset, (1 + offset) * stepCost });
                        }
                        offset++;
                    }

                    offset = 1;
                    while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                    {
                        if (!TopOfBin[topRowPos + offset])
                        {
                            Moves.Add(new[] { (bin + 1) * 100, topRowPos + offset, (1 + offset) * stepCost });
                        }

                        offset++;
                    }
                }

                // Bottom of bins
                if (TopRow[(bin * 2) + 2] == 0 && Bins[bin][0] == 0 && Bins[bin][1] != (bin + 1) && Bins[bin][1] != 0)
                {
                    // Move it out
                    var topRowPos = (bin * 2) + 2;
                    var stepCost = Cost(Bins[bin][1]);
                    var offset = 1;

                    while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                    {
                        Moves.Add(new[] { (bin + 1) * 100 + 1, topRowPos - offset, (2 + offset) * stepCost });
                        offset++;
                    }

                    offset = 1;
                    while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                    {
                        Moves.Add(new[] { (bin + 1) * 100 + 1, topRowPos + offset, (2 + offset) * stepCost });
                        offset++;
                    }
                }

                // From hallway to bottom of bins
                if (TopRow[(bin * 2) + 2] == 0 && Bins[bin][0] == 0 && Bins[bin][1] == 0)
                {
                    // Find one to move in
                    var topRowPos = (bin * 2) + 2;
                    var stepCost = Cost(bin + 1);
                    var offset = 1;

                    while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                    {
                        offset++;
                    }

                    if (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == (bin + 1))
                    {
                        Moves.Add(new[] { topRowPos - offset, (bin + 1) * 100 + 1, (2 + offset) * stepCost });
                    }

                    offset = 1;
                    while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                    {
                        offset++;
                    }

                    if (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == (bin + 1))
                    {
                        Moves.Add(new[] { topRowPos + offset, (bin + 1) * 100 + 1, (2 + offset) * stepCost });
                    }
                }

                // From hallway to top of bins
                if (TopRow[(bin * 2) + 2] == 0 && Bins[bin][1] == (bin + 1) && Bins[bin][0] == 0)
                {
                    // Find one to move in
                    var topRowPos = (bin * 2) + 2;
                    var stepCost = Cost(bin + 1);
                    var offset = 1;

                    while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                    {
                        offset++;
                    }

                    if (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == (bin + 1))
                    {
                        Moves.Add(new[] { topRowPos - offset, (bin + 1) * 100, (1 + offset) * stepCost });
                    }

                    offset = 1;
                    while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                    {
                        offset++;
                    }

                    if (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == (bin + 1))
                    {
                        Moves.Add(new[] { topRowPos + offset, (bin + 1) * 100, (1 + offset) * stepCost });
                    }
                }

            }
        }

        public int MinimumEnergyToSolve()
        {
            int minE = 0;

            for (int i = 0; i < 4; i++)
            {
                if (Bins[i][0] != (i + 1))
                {
                    minE += 2 * Cost(i + 1);
                }

                if (Bins[i][1] != (i + 1))
                {
                    minE += 3 * Cost(i + 1);
                }
            }

            return minE;
        }

        public void Print()
        {
            Console.WriteLine("#############");
            Console.WriteLine("#" + new string(TopRow.Select(p => ".ABCD"[p]).ToArray()) + $"#  {Evaluation}");
            Console.WriteLine("###" + ".ABCD"[Bins[0][0]] + "#" + ".ABCD"[Bins[1][0]] + "#" + ".ABCD"[Bins[2][0]] + "#" + ".ABCD"[Bins[3][0]] + "###");
            Console.WriteLine("  #" + ".ABCD"[Bins[0][1]] + "#" + ".ABCD"[Bins[1][1]] + "#" + ".ABCD"[Bins[2][1]] + "#" + ".ABCD"[Bins[3][1]] + "#");
            Console.WriteLine("  #########");
        }

        public void Make(int[] move)
        {
            if (move[0] >= 100)
            {
                int bin = (move[0] / 100) - 1;
                int pos = move[0] % 100;

                TopRow[move[1]] = Bins[bin][pos];
                Bins[bin][pos] = 0;

                Evaluation += move[2];
            }
            else
            {
                int bin = (move[1] / 100) - 1;
                int pos = move[1] % 100;

                Bins[bin][pos] = TopRow[move[0]];
                TopRow[move[0]] = 0;

                Evaluation += move[2];
            }
        }

        public void Retract(int[] move)
        {
            if (move[0] >= 100)
            {
                int bin = (move[0] / 100) - 1;
                int pos = move[0] % 100;

                Bins[bin][pos] = TopRow[move[1]];
                TopRow[move[1]] = 0;

                Evaluation -= move[2];
            }
            else
            {
                int bin = (move[1] / 100) - 1;
                int pos = move[1] % 100;

                TopRow[move[0]] = Bins[bin][pos];
                Bins[bin][pos] = 0;

                Evaluation -= move[2];
            }
        }

        public void ClearMoves()
        {
            Moves = new List<int[]>();
        }
    }

    internal class Board23_2
    {
        public int[] TopRow;

        public int[][] Bins;

        public List<int[]> Moves = new List<int[]>();

        public int Evaluation { get; set; }

        public bool[] TopOfBin = new[] { false, false, true, false, true, false, true, false, true, false, false };

        // Positions:
        // 0  1  2  3  4  5  6  7  8  9 10
        //     100   200   300   400
        //     101   201   301   401

        public Board23_2()
        {
            TopRow = new int[11];

            Bins = new[]
            {
                new[] { 1, 4, 4, 4 },
                new[] { 3, 3, 2, 1 },
                new[] { 2, 2, 1, 4 },
                new[] { 3, 1, 3, 2 }
            };
        }

        public void InitTestPattern()
        {
            Bins = new[]
            {
                new[] { 2, 4, 4, 1 },
                new[] { 3, 3, 2, 4 },
                new[] { 2, 2, 1, 3 },
                new[] { 4, 1, 3, 1 }
            };
        }

        public int Cost(int piece)
        {
            switch (piece)
            {
                case 1:
                    return 1;
                case 2:
                    return 10;
                case 3:
                    return 100;
                case 4:
                    return 1000;
            }

            return 0;
        }

        public bool Done()
        {
            for (int bin = 0; bin < 4; bin++)
            {
                for (int d = 0; d < 4; d++)
                {
                    if (Bins[bin][d] != (bin + 1))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Board23_2(Board23_2 other)
        {
            TopRow = other.TopRow.Select(c => c).ToArray();
            Bins = other.Bins.Select(b => b.Select(v => v).ToArray()).ToArray();
            Evaluation = other.Evaluation;
        }

        private bool EmptyAbove(int bin, int depth)
        {
            if (TopRow[(bin*2) + 2] != 0)
            {
                return false;
            }

            for (int i = 0; i < depth; i++)
            {
                if (Bins[bin][i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void GenerateMoves()
        {
            // Iterate over bins
            for (int bin = 3; bin >= 0; bin--)
            {
                for (int d = 0; d < 4; d++)
                {
                    // From bins to hallway
                    if (EmptyAbove(bin, d) && Bins[bin].Skip(d).All(v => v != 0) && Bins[bin].Skip(d).Any(v => v != (bin + 1)))
                    {
                        // Move it out
                        var topRowPos = (bin * 2) + 2;
                        var stepCost = Cost(Bins[bin][d]);
                        var offset = 1;

                        while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                        {
                            if (!TopOfBin[topRowPos - offset])
                            {
                                Moves.Add(new[] { (bin + 1) * 100 + d, topRowPos - offset, (d + 1 + offset) * stepCost });
                            }
                            offset++;
                        }

                        offset = 1;
                        while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                        {
                            if (!TopOfBin[topRowPos + offset])
                            {
                                Moves.Add(new[] { (bin + 1) * 100 + d, topRowPos + offset, (d + 1 + offset) * stepCost });
                            }
                            offset++;
                        }
                    }

                    // From hallway to bins
                    if (EmptyAbove(bin, d) && Bins[bin][d] == 0 && (d == 3 || Bins[bin].Skip(d + 1).All(v => v == (bin + 1))))
                    {
                        // Find one to move in
                        var topRowPos = (bin * 2) + 2;
                        var stepCost = Cost(bin + 1);
                        var offset = 1;

                        while (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == 0)
                        {
                            offset++;
                        }

                        if (topRowPos - offset >= 0 && TopRow[topRowPos - offset] == (bin + 1))
                        {
                            Moves.Add(new[] { topRowPos - offset, (bin + 1) * 100 + d, (d + 1 + offset) * stepCost });
                        }

                        offset = 1;
                        while (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == 0)
                        {
                            offset++;
                        }

                        if (topRowPos + offset <= 10 && TopRow[topRowPos + offset] == (bin + 1))
                        {
                            Moves.Add(new[] { topRowPos + offset, (bin + 1) * 100 + d, (d + 1 + offset) * stepCost });
                        }
                    }
                }
            }
        }

        public int MinimumEnergyToSolve()
        {
            int minE = 0;

            for (int i = 0; i < 4; i++)
            {
                var c = Cost(i + 1);

                for (int d = 0; d < 4; d++)
                {
                    if (Bins[i][d] == 0)
                    {
                        minE += (d + 1 + 1) * c;
                    }
                    else if (Bins[i][d] != (i + 1))
                    {
                        minE += (d + 1 + 3) * c;
                    }
                }
            }

            return minE;
        }

        public void Print()
        {
            Console.WriteLine("#############");
            Console.WriteLine("#" + new string(TopRow.Select(p => ".ABCD"[p]).ToArray()) + $"#  {Evaluation}");
            Console.WriteLine("###" + ".ABCD"[Bins[0][0]] + "#" + ".ABCD"[Bins[1][0]] + "#" + ".ABCD"[Bins[2][0]] + "#" + ".ABCD"[Bins[3][0]] + "###");
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("  #" + ".ABCD"[Bins[0][i]] + "#" + ".ABCD"[Bins[1][i]] + "#" + ".ABCD"[Bins[2][i]] + "#" + ".ABCD"[Bins[3][i]] + "#");
            }
            Console.WriteLine("  #########");
        }

        public void Make(int[] move)
        {
            if (move[0] >= 100)
            {
                int bin = (move[0] / 100) - 1;
                int pos = move[0] % 100;

                TopRow[move[1]] = Bins[bin][pos];
                Bins[bin][pos] = 0;

                Evaluation += move[2];
            }
            else
            {
                int bin = (move[1] / 100) - 1;
                int pos = move[1] % 100;

                Bins[bin][pos] = TopRow[move[0]];
                TopRow[move[0]] = 0;

                Evaluation += move[2];
            }
        }

        public void Retract(int[] move)
        {
            if (move[0] >= 100)
            {
                int bin = (move[0] / 100) - 1;
                int pos = move[0] % 100;

                Bins[bin][pos] = TopRow[move[1]];
                TopRow[move[1]] = 0;

                Evaluation -= move[2];
            }
            else
            {
                int bin = (move[1] / 100) - 1;
                int pos = move[1] % 100;

                TopRow[move[0]] = Bins[bin][pos];
                Bins[bin][pos] = 0;

                Evaluation -= move[2];
            }
        }

        public void ClearMoves()
        {
            Moves = new List<int[]>();
        }
    }



    internal class Day23
    {
        public int PrevMin = 9999999;

        public void Search(Board23 b)
        {
            if (b.Done())
            {
                if (b.Evaluation < PrevMin)
                {
                    PrevMin = b.Evaluation;
                }
                return;
            }

            if (b.Evaluation + b.MinimumEnergyToSolve() > PrevMin)
            {
                // No need to search
                return;
            }

            b.GenerateMoves();

            if (b.Moves.Count == 0)
            {
                return;
            }

            foreach (var move in b.Moves)
            {
                // Make all moves and check min eval
                if (b.Evaluation + move[2] < PrevMin)
                {
                    var newb = new Board23(b);
                    newb.Make(move);
                    //newb.Print();
                    Console.WriteLine($"Current min = {PrevMin}");
                    Search(newb);
                }
            }
        }

        public void Search2(Board23_2 b)
        {
            if (b.Done())
            {
                if (b.Evaluation < PrevMin)
                {
                    PrevMin = b.Evaluation;
                }
                return;
            }

            if (b.Evaluation + b.MinimumEnergyToSolve() > PrevMin)
            {
                // No need to search
                return;
            }

            b.GenerateMoves();

            if (b.Moves.Count == 0)
            {
                return;
            }

            foreach (var move in b.Moves)
            {
                // Make all moves and check min eval
                if (b.Evaluation + move[2] < PrevMin)
                {
                    var newb = new Board23_2(b);
                    newb.Make(move);
                    //newb.Print();
                    Console.WriteLine($"Current min = {PrevMin}");
                    Search2(newb);
                }
            }
        }

        public void Part1()
        {
            Board23 b = new Board23();

            //b.InitTestPattern();

            b.Print();
           
            Search(b);

            Console.WriteLine($"Min cost is {PrevMin}");
        }

        public void Part2()
        {
            Board23_2 b = new Board23_2();

            //b.InitTestPattern();

            b.Print();

            Search2(b);

            Console.WriteLine($"Min cost is {PrevMin}");
        }

    }
}
