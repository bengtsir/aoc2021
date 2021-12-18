using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace aoc2021
{
    internal class Day17
    {
        private int xStart;
        private int xEnd;
        private int yStart;
        private int yEnd;

        private int currentX;
        private int currentY;

        private int xVel;
        private int yVel;

        private List<int[]> initVelocities = new  List<int[]>();

        private int TargetCompare(int x, int y)
        {
            if (y < yStart)
            {
                return -2;
            }

            if (x < xStart)
            {
                return -1;
            }

            if (x > xEnd)
            {
                return +1;
            }

            if (y > yEnd)
            {
                return +2;
            }

            return 0;
        }

        void Step()
        {
            currentX += xVel;
            currentY += yVel;

            if (xVel > 0)
            {
                xVel--;
            }
            else if (xVel < 0)
            {
                xVel++;
            }

            yVel -= 1;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day17.txt");

            var re = new Regex(@".*x=(?<startx>[\d]+)\.\.(?<endx>[\d]+), y=(?<starty>[\d-]+)\.\.(?<endy>[\d-]+)$");

            var m = re.Match(data[0]);

            xStart = Int32.Parse(m.Groups["startx"].Value);
            xEnd = Int32.Parse(m.Groups["endx"].Value);
            yStart = Int32.Parse(m.Groups["starty"].Value);
            yEnd = Int32.Parse(m.Groups["endy"].Value);

            // First scan for suitable x range
            var xVelMax = xEnd;
            var xVelMin = (int) Math.Sqrt(xStart);

            var maxYpos = yStart - 100;

            for (var initxVel = xVelMin; initxVel <= xVelMax; initxVel++)
            {
                for (int inityVel = yStart; inityVel <= -yStart + 100; inityVel++)
                {
                    xVel = initxVel;
                    yVel = inityVel;

                    currentX = 0;
                    currentY = 0;

                    var lastX = currentX;
                    var lastY = currentY;

                    var localYmax = currentY;

                    bool stop = false;
                    while (!stop)
                    {
                        Step();
                        if (currentY > localYmax)
                        {
                            localYmax = currentY;
                        }

                        var c = TargetCompare(currentX, currentY);

                        switch (c)
                        {
                            case -2:
                                stop = true;
                                break;
                            case -1:
                                if (currentX == lastX)
                                {
                                    stop = true;
                                }
                                break;
                            case 0:
                                if (localYmax > maxYpos)
                                {
                                    maxYpos = localYmax;
                                    Console.WriteLine($"Found new max Y: {maxYpos} with init x vel = {initxVel}, init y vel = {inityVel}");
                                    stop = true;
                                }

                                if (!initVelocities.Any(v => v[0] == initxVel && v[1] == inityVel))
                                {
                                    initVelocities.Add(new int[] { initxVel, inityVel });
                                }
                                break;
                            case 1:
                                stop = true;
                                break;
                            case 2:
                                // Do nothing
                                break;
                        }

                        lastX = currentX;
                        lastY = currentY;
                    }
                }
            }
        }

        public void Part2()
        {
            Part1();

            Console.WriteLine();

            var points = string.Join(" ", initVelocities.Select(p => $"{p[0]},{p[1]}").ToArray());
            Console.WriteLine($"Found these init velocities: {points}");
            Console.WriteLine($"Number of distinct velocities: {initVelocities.Count}");
        }
    }
}
