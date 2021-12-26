using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day25
    {
        internal int Iterate(char[][] state, out char[][] newState)
        {
            newState = state.Select(r => r.Select(v => v).ToArray()).ToArray();

            int moves = 0;

            int xSize = state[0].Length;
            int ySize = state.Length;

            for (int i = 0; i < ySize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    if (state[i][j] == '>' && state[i][(j + 1) % xSize] == '.')
                    {
                        newState[i][j] = '.';
                        newState[i][(j + 1) % xSize] = '>';
                        moves++;
                    }
                }
            }

            state = newState;
            newState = state.Select(r => r.Select(v => v).ToArray()).ToArray();

            for (int i = 0; i < ySize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    if (state[i][j] == 'v' && state[(i + 1) % ySize][j] == '.')
                    {
                        newState[i][j] = '.';
                        newState[(i + 1) % ySize][j] = 'v';
                        moves++;
                    }
                }
            }

            return moves;
        }

        void PrintState(char[][] state)
        {
            foreach (var r in state)
            {
                Console.WriteLine(new string(r));
            }
            Console.WriteLine();
        }

        public void Part1()
        {
            var testData = new string[]
            {
                "v...>>.vv>",
                ".vv>>.vv..",
                ">>.>v>...v",
                ">>v>>.>.v.",
                "v>v.vv.v..",
                ">.>>..v...",
                ".vv..>.>v.",
                "v.v..>>v.v",
                "....v..v.>",
            };

            var data = File.ReadAllLines(@"data\day25.txt");

            var state = data.Select(r => r.Select(c => c).ToArray()).ToArray();
            //var state = testData.Select(r => r.Select(c => c).ToArray()).ToArray();

            char[][] newState;

            var iteration = 0;

            PrintState(state);

            while (Iterate(state, out newState) > 0)
            {
                iteration++;
                state = newState;
                PrintState(state);
            }

            Console.WriteLine($"Stable state after {iteration} steps");
        }

    }
}
