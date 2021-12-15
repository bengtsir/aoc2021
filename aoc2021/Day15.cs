using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Day15
    {
        private long lowestRisk = 99999999999999999;
        private int xSize;
        private int ySize;

        private int[][] values;
        private bool[][] visited;
        private int[][] risks;

        internal void Visit(int x, int y)
        {
            bool stop = false;

            var counter = 0;

            while (!stop)
            {
                counter++;
                if (counter > 100)
                {
                    counter = 0;
                    Console.WriteLine($"Checking ({y}, {x}) with value {risks[y][x]}");
                }

                if (y < ySize - 1 && !visited[y + 1][x])
                {
                    var s = risks[y][x] + values[y + 1][x];
                    if (s < risks[y+1][x])
                    {
                        risks[y + 1][x] = s;
                    }
                }

                if (x < xSize - 1 && !visited[y][x + 1])
                {
                    var s = risks[y][x] + values[y][x + 1];
                    if (s < risks[y][x + 1])
                    {
                        risks[y][x + 1] = s;
                    }
                }

                if (y > 0 && !visited[y - 1][x])
                {
                    var s = risks[y][x] + values[y - 1][x];
                    if (s < risks[y - 1][x])
                    {
                        risks[y - 1][x] = s;
                    }
                }

                if (x > 0 && !visited[y][x - 1])
                {
                    var s = risks[y][x] + values[y][x - 1];
                    if (s < risks[y][x - 1])
                    {
                        risks[y][x - 1] = s;
                    }
                }

                visited[y][x] = true;

                if (y == ySize - 1 && x == xSize - 1)
                {
                    return;
                }

                var newx = -1;
                var newy = -1;
                var leastVal = 999999999;

                for (int yy = 0; yy < ySize; yy++)
                {
                    for (int xx = 0; xx < xSize; xx++)
                    {
                        if (!visited[yy][xx] && risks[yy][xx] < leastVal)
                        {
                            leastVal = risks[yy][xx];
                            newx = xx;
                            newy = yy;
                        }
                    }
                }

                if (newx < 0 || newy < 0)
                {
                    Console.WriteLine($"error");
                }

                x = newx;
                y = newy;
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day15.txt");

            values = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();
            visited = data.Select(r => r.Select(c => false).ToArray()).ToArray();
            risks = data.Select(r => r.Select(c => 999999999).ToArray()).ToArray();

            xSize = data[0].Length;
            ySize = data.Length;

            risks[0][0] = 0;

            Visit(0, 0);

            Console.WriteLine($"Lowest risk: {risks[ySize-1][xSize-1]}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day15.txt");

            var initValues = data.Select(r => r.Select(c => Int32.Parse(c.ToString())).ToArray()).ToArray();

            xSize = data[0].Length;
            ySize = data.Length;

            values = new int[ySize*5][];

            for (int yy = 0; yy < 5; yy++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    values[yy * ySize + y] = new int[xSize * 5];
                    for (int xx = 0; xx < 5; xx++)
                    {
                        for (int x = 0; x < xSize; x++)
                        {
                            values[yy * ySize + y][xx * xSize + x] = (initValues[y][x] + xx + yy);
                            if (values[yy * ySize + y][xx * xSize + x] > 9)
                            {
                                values[yy * ySize + y][xx * xSize + x] -= 9;
                            }
                        }
                    }
                }
            }

            visited = values.Select(r => r.Select(c => false).ToArray()).ToArray();
            risks = values.Select(r => r.Select(c => 999999999).ToArray()).ToArray();

            xSize = values[0].Length;
            ySize = values.Length;

            risks[0][0] = 0;

            Visit(0, 0);

            Console.WriteLine($"Lowest risk: {risks[ySize - 1][xSize - 1]}");
        }
    }
}