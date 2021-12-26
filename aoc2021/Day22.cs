using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Cube22
    {
        public long[][] Coords { get; set; }

        private bool Intersects(long[] a, long[] b)
        {
            return (a[0] >= b[0] && a[0] <= b[1] ||
                    a[1] >= b[0] && a[1] <= b[1] ||
                    b[0] >= a[0] && b[0] <= a[1] ||
                    b[1] >= a[0] && b[1] <= a[1]);
        }

        public bool Intersects(Cube22 other)
        {
            return Intersects(Coords[0], other.Coords[0]) &&
                   Intersects(Coords[1], other.Coords[1]) &&
                   Intersects(Coords[2], other.Coords[2]);
        }

        public long Size => (Coords[0][1] - Coords[0][0] + 1) *
                            (Coords[1][1] - Coords[1][0] + 1) *
                            (Coords[2][1] - Coords[2][0] + 1);

        public override string ToString()
        {
            return $"{Coords[0][0]}..{Coords[0][1]} {Coords[1][0]}..{Coords[1][1]} {Coords[2][0]}..{Coords[2][1]} ({Size})";
        }

        public Cube22()
        {
            // Nothing
        }

        public Cube22(Cube22 other)
        {
            Coords = other.Coords.Select(d => d.Select(x => x).ToArray()).ToArray();
        }

        public Cube22(string def)
        {
            Regex re = new Regex(@"^x=([\d-]+)\.\.([\d-]+),y=([\d-]+)\.\.([\d-]+),z=([\d-]+)\.\.([\d-]+)$");

            var m = re.Match(def);

            if (!m.Success)
            {
                throw new Exception($"Invalid format: {def}");
            }

            Coords = new long[][]
            {

                new long[] { Int32.Parse(m.Groups[1].Value), Int32.Parse(m.Groups[2].Value) },
                new long[] { Int32.Parse(m.Groups[3].Value), Int32.Parse(m.Groups[4].Value) },
                new long[] { Int32.Parse(m.Groups[5].Value), Int32.Parse(m.Groups[6].Value) },
            };
        }

        // Removes the overlapping sections of this cube with other
        public List<Cube22> Sub(Cube22 other)
        {
            List<Cube22> res = new List<Cube22>() { this };

            if (!Intersects(other))
            {
                return res;
            }

            for (int dim = 0; dim < 3; dim++)
            {
                var newList = new List<Cube22>();

                foreach (var cube in res)
                {
                    if (other.Coords[dim][0] <= cube.Coords[dim][0] && other.Coords[dim][1] >= cube.Coords[dim][1])
                    {
                        //      A____B
                        //    a_________b
                        newList.Add(new Cube22(cube));
                    }
                    else if (other.Coords[dim][0] <= cube.Coords[dim][0] && other.Coords[dim][1] < cube.Coords[dim][1])
                    {
                        //      A_____B
                        //    a_____b
                        var c = new Cube22(cube);
                        c.Coords[dim][1] = other.Coords[dim][1];
                        newList.Add(c);

                        c = new Cube22(cube);
                        c.Coords[dim][0] = other.Coords[dim][1] + 1;
                        newList.Add(c);
                    }
                    else if (other.Coords[dim][0] > cube.Coords[dim][0] && other.Coords[dim][1] >= cube.Coords[dim][1])
                    {
                        //    A_____B
                        //       a_____b
                        var c = new Cube22(cube);
                        c.Coords[dim][1] = other.Coords[dim][0] - 1;
                        newList.Add(c);

                        c = new Cube22(cube);
                        c.Coords[dim][0] = other.Coords[dim][0];
                        newList.Add(c);
                    }
                    else
                    {
                        //   A__________B
                        //      a___b
                        var c = new Cube22(cube);
                        c.Coords[dim][1] = other.Coords[dim][0] - 1;
                        newList.Add(c);

                        c = new Cube22(cube);
                        c.Coords[dim][0] = other.Coords[dim][0];
                        c.Coords[dim][1] = other.Coords[dim][1];
                        newList.Add(c);

                        c = new Cube22(cube);
                        c.Coords[dim][0] = other.Coords[dim][1] + 1;
                        newList.Add(c); // slkjdfskljfdskljdöfljskljkljsdfklj!!!!!!!
                    }
                }

                res = newList;
            }

            var filtered = res.Where(c => !c.Intersects(other)).ToList();

            if (filtered.Count != res.Count - 1)
            {
                throw new Exception("Something is rotten in the state of Cube22");
            }

            return filtered;
        }
    }

    internal class Day22
    {
        private List<Cube22> Cubes = new List<Cube22>();

        internal void HandleRow(string s)
        {
            var words = s.Split(' ');
            var c = new Cube22(words[1]);

            var newC = new List<Cube22>();

            newC = Cubes.SelectMany(oc => oc.Sub(c)).ToList();

            if (words[0] == "on")
            {
                newC.Add(c);
            }

            Cubes = newC;
        }

        private int[] ones = new int[101 * 101 * 101];

        internal void HandleRow2(string s)
        {
            var words = s.Split(' ');

            var c = new Cube22(words[1]);

            var val = words[0] == "on" ? 1 : 0;

            for (long i = c.Coords[0][0]; i <= c.Coords[0][1]; i++)
            {
                for (long j = c.Coords[1][0]; j <= c.Coords[1][1]; j++)
                {
                    for (long k = c.Coords[2][0]; k <= c.Coords[2][1]; k++)
                    {
                        ones[(i + 50) * 101 * 101 + (j + 50) * 101 + (k + 50)] = val;
                    }
                }
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day22.txt");

            var test1 = new string[]
            {
                "on x=10..12,y=10..12,z=10..12",
                "on x=11..13,y=11..13,z=11..13",
                "off x=9..11,y=9..11,z=9..11",
                "on x=10..10,y=10..10,z=10..10",
            };

            foreach (var s in test1)
            {
                HandleRow(s);
                HandleRow2(s);
                Console.WriteLine($"Subsum is {Cubes.Sum(c => c.Size)}");
                Console.WriteLine($"Subsum with other is {ones.Sum()}");
            }

            var size1 = Cubes.Sum(c => c.Size);

            Console.WriteLine($"test 1 size is {size1}");

            Cubes = new List<Cube22>();
            ones = new int[101 * 101 * 101];

            var test2 = new string[]
            {
                "on x=-20..26,y=-36..17,z=-47..7",
                "on x=-20..33,y=-21..23,z=-26..28",
                "on x=-22..28,y=-29..23,z=-38..16",
                "on x=-46..7,y=-6..46,z=-50..-1",
                "on x=-49..1,y=-3..46,z=-24..28",
                "on x=2..47,y=-22..22,z=-23..27",
                "on x=-27..23,y=-28..26,z=-21..29",
                "on x=-39..5,y=-6..47,z=-3..44",
                "on x=-30..21,y=-8..43,z=-13..34",
                "on x=-22..26,y=-27..20,z=-29..19",
                "off x=-48..-32,y=26..41,z=-47..-37",
                "on x=-12..35,y=6..50,z=-50..-2",
                "off x=-48..-32,y=-32..-16,z=-15..-5",
                "on x=-18..26,y=-33..15,z=-7..46",
                "off x=-40..-22,y=-38..-28,z=23..41",
                "on x=-16..35,y=-41..10,z=-47..6",
                "off x=-32..-23,y=11..30,z=-14..3",
                "on x=-49..-5,y=-3..45,z=-29..18",
                "off x=18..30,y=-20..-8,z=-3..13",
                "on x=-41..9,y=-7..43,z=-33..15",
            };

            foreach (var s in test2)
            {
                if (s.StartsWith("off"))
                {
                    var ll = new Cube22(s.Split(' ')[1]);

                    Console.WriteLine("Cubes before:");
                    foreach (var c in Cubes)
                    {
                        Console.WriteLine(c + " " + (c.Intersects(ll) ? "Match" : ""));
                    }
                }

                HandleRow(s);
                Console.WriteLine($"Subsum after {s} is {Cubes.Sum(c => c.Size)}");

                if (s.StartsWith("off"))
                {
                    Console.WriteLine("Cubes after:");
                    foreach (var c in Cubes)
                    {
                        Console.WriteLine(c);
                    }
                }

                HandleRow2(s);
                Console.WriteLine($"Subsum with other is {ones.Sum()}");
            }

            var size2 = Cubes.Sum(c => c.Size);

            Console.WriteLine($"test 2 size is {size2}");

            Cubes = new List<Cube22>();
            ones = new int[101 * 101 * 101];

            foreach (var s in data.Take(20))
            {
                HandleRow(s);
                Console.WriteLine($"Subsum after {s} is {Cubes.Sum(c => c.Size)}");

                HandleRow2(s);
                Console.WriteLine($"Subsum with other is {ones.Sum()}");
            }

            var size3 = Cubes.Sum(c => c.Size);

            Console.WriteLine($"Part 1 size is {size3}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day22.txt");

            foreach (var s in data)
            {
                HandleRow(s);
                Console.WriteLine($"Subsum is {Cubes.Sum(c => c.Size)}");
            }

            var size1 = Cubes.Sum(c => c.Size);

            Console.WriteLine($"Final size is {size1}");
        }
    }
}
