using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Die21
    {
        private int lastVal = 0;
        private int rolls = 0;

        public int Next
        {
            get
            {
                lastVal++;
                rolls++;
                if (lastVal > 100)
                {
                    lastVal = 1;
                }

                return lastVal;
            }
        }

        public int Next3 => Next + Next + Next;

        public int Rolls => rolls;
    }

    internal class Day21
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day21.txt");

            var die = new Die21();

            int pos1 = 3;
            int pos2 = 1;

            int score1 = 0;
            int score2 = 0;

            bool flip = true;

            while (score1 < 1000 && score2 < 1000)
            {
                if (flip)
                {
                    pos1 = (pos1 + die.Next3) % 10;
                    score1 += pos1 + 1;
                }
                else
                {
                    pos2 = (pos2 + die.Next3) % 10;
                    score2 += pos2 + 1;
                }

                flip = !flip;
            }

            int result = (score1 >= 1000 ? score2 : score1) * die.Rolls;

            Console.WriteLine($"Final score is {score1} - {score2}, total rolls is {die.Rolls}, result is {result}");
        }

        internal int[][] Moves = new int[][]
        {
            new int[] { 3, 1 },
            new int[] { 4, 3 },
            new int[] { 5, 6 },
            new int[] { 6, 7 },
            new int[] { 7, 6 },
            new int[] { 8, 3 },
            new int[] { 9, 1 },
        };

        // thisScore, otherScore, thisPos, otherPos, thisWins/otherWins
        long[,,,,] Wins = new long[30,30,10,10,2];

        void SearchWins()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    SearchWinsInt(0, 0, i, j);
                }
            }
        }

        void SearchWinsInt(int thisScore, int otherScore, int thisPos, int otherPos)
        {
            long ww = 0;
            long bw = 0;

            foreach (var move in Moves)
            {
                var newPos = (thisPos + move[0]) % 10;
                var newScore = thisScore + newPos + 1;

                if (newScore >= 21)
                {
                    ww += move[1] * 1;
                    bw += 0;
                }
                else
                {
                    var sww = Wins[otherScore, newScore, otherPos, newPos, 0];
                    var sbw = Wins[otherScore, newScore, otherPos, newPos, 1];

                    if (sww + sbw == 0)
                    {
                        SearchWinsInt(otherScore, newScore, otherPos, newPos);
                        
                        sww = Wins[otherScore, newScore, otherPos, newPos, 0];
                        sbw = Wins[otherScore, newScore, otherPos, newPos, 1];
                    }

                    ww += move[1] * sbw;
                    bw += move[1] * sww;
                }
            }

            Wins[thisScore, otherScore, thisPos, otherPos, 0] = ww;
            Wins[thisScore, otherScore, thisPos, otherPos, 1] = bw;
        }

        public void Part2()
        {
            SearchWins();

            long ww = Wins[0, 0, 3, 1, 0];
            long bw = Wins[0, 0, 3, 1, 1];

            long tww = Wins[0, 0, 3, 7, 0]; // Should be 444356092776315
            long tbw = Wins[0, 0, 3, 7, 1]; // Should be 341960390180808

            Console.WriteLine($"White wins: {ww} Black wins: {bw}, largest = {Math.Max(ww, bw)}");
        }
    }
}
