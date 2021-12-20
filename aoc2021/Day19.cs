using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Scanner19
    {
        public List<int[]> Points { get; } = new List<int[]>();
        public int Number { get; set; }
        public int[] Offset { get; set; }
        public int[,] Transform { get; set; }
        public bool Done { get; set; } = false;
    }

    internal class Result19
    {
        public int[,] Transform { get; set; }
        public int[] Offset { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }

    internal class Day19
    {
        internal List<Scanner19> Scanners = new List<Scanner19>();

        internal List<int[,]> transforms = new List<int[,]>();

        internal List<Result19> Connections = new List<Result19>();

        
        public Day19()
        {
            GenerateTransforms();
        }

        public int[,] MultiplyMatrix(int[,] A, int[,] B)
        {
            var rA = A.GetLength(0);
            var cA = A.GetLength(1);
            var rB = B.GetLength(0);
            var cB = B.GetLength(1);

            int temp = 0;

            int[,] res = new int[rA, cB];

            if (cA != rB)
            {
                throw new Exception("matrix can't be multiplied !!");
            }
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    temp = 0;
                    for (int k = 0; k < cA; k++)
                    {
                        temp += A[i, k] * B[k, j];
                    }

                    res[i, j] = temp;
                }
            }

            return res;
        }

        public int[] Transform(int[,] t, int[] p)
        {
            return new int[]
            {
                t[0, 0] * p[0] + t[0, 1] * p[1] + t[0, 2] * p[2],
                t[1, 0] * p[0] + t[1, 1] * p[1] + t[1, 2] * p[2],
                t[2, 0] * p[0] + t[2, 1] * p[1] + t[2, 2] * p[2],
            };
        }


        private int[,] id = new int[,]
        {
            {1, 0, 0}, {0, 1, 0}, {0, 0, 1}
        };

        private int[,] rx90 = new int[,]
        {
            { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 }
        };

        private int[,] ry90 = new int[,]
        {
            { 0, 0, 1 }, { 0, 1, 0 }, { -1, 0, 0 }
        };

        private int[,] rz90 = new int[,]
        {
            { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 }
        };

        internal void GenerateSubTransforms(int[,] m)
        {
            transforms.Add(m);
            m = MultiplyMatrix(m, rx90);
            transforms.Add(m);
            m = MultiplyMatrix(m, rx90);
            transforms.Add(m);
            m = MultiplyMatrix(m, rx90);
            transforms.Add(m);
        }

        internal void GenerateTransforms()
        {
            var m = MultiplyMatrix(id, id); // Create a copy ugly way...
            GenerateSubTransforms(m);

            m = MultiplyMatrix(m, ry90);
            GenerateSubTransforms(m);
            m = MultiplyMatrix(m, ry90);
            GenerateSubTransforms(m);
            m = MultiplyMatrix(m, ry90);
            GenerateSubTransforms(m);

            m = MultiplyMatrix(id, rz90);
            GenerateSubTransforms(m);
            m = MultiplyMatrix(MultiplyMatrix(m, rz90), rz90);
            GenerateSubTransforms(m);
        }

        internal void Parse(string[] data)
        {
            var scanner = new Scanner19();

            foreach (var line in data)
            {
                if (line.StartsWith("---"))
                {
                    scanner = new Scanner19
                    {
                        Number = int.Parse(line.Substring(12, 2))
                    };
                }
                else if (line.Length < 2)
                {
                    Scanners.Add(scanner);
                }
                else
                {
                    scanner.Points.Add(line.Split(',').Select(Int32.Parse).ToArray());
                }
            }
        }

        int[] Diff(int[] a, int[] b)
        {
            return a.Zip(b, (x, y) => x - y).ToArray();
        }

        int[] Add(int[] a, int[] o)
        {
            return a.Zip(o, (x, y) => x + y).ToArray();
        }

        int ManhattanDist(int[] a, int[] b)
        {
            return a.Zip(b, (x, y) => Math.Abs(x - y)).Sum();
        }

        internal bool Compare(Scanner19 a, Scanner19 b)
        {
            foreach (var transform in transforms)
            {
                if (Compare_int(a, b, transform, out var ofs))
                {
                    b.Transform = transform;
                    b.Offset = ofs;

                    //AddPoints(a, b);

                    b.Done = true;

                    return true;
                }
            }

            return false;
        }

        internal bool Compare_int(Scanner19 a, Scanner19 b, int[,] transform, out int[] offset)
        {
            var btp = b.Points.Select(p => Transform(transform, p)).ToList();

            foreach (var p1 in a.Points)
            {
                foreach (var p2 in btp.Skip(11)) // Need 12 matches, only need to check n - 11 offsets here
                {
                    var o = Diff(p1, p2);

                    if (btp.Sum(p => a.Points.Count(ap => Diff(ap, p).SequenceEqual(o))) >= 12)
                    {
                        // Gotcha
                        offset = o;
                        return true;
                    }
                }
            }

            offset = new int[] { 0, 0, 0 };
            return false;
        }

        internal void AddPoints(Scanner19 a, Scanner19 b)
        {
            var btp = b.Points.Select(p => Add(Transform(b.Transform, p), b.Offset)).ToList();

            var bb = btp.Where(bp => !a.Points.Any(p => p.SequenceEqual(bp)));
            
            a.Points.AddRange(bb);
        }

        internal void Follow(int[,] prevMatrix, int[] prevOffset, Result19 conn)
        {
            if (Scanners[conn.To].Done)
            {
                return;
            }

            var xf = MultiplyMatrix(prevMatrix, conn.Transform);
            var of = Add(prevOffset, Transform(prevMatrix, conn.Offset));

            Scanners[conn.To].Transform = xf;
            Scanners[conn.To].Offset = of;

            AddPoints(Scanners[0], Scanners[conn.To]);

            Scanners[conn.To].Done = true;

            foreach (var newConn in Connections.Where(c => c.From == conn.To))
            {
                Follow(xf, of, newConn);
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day19.txt");

            /*
            data = new string[]
            {
                "--- scanner 0 ---",
                "404,-588,-901",
                "528,-643,409",
                "-838,591,734",
                "390,-675,-793",
                "-537,-823,-458",
                "-485,-357,347",
                "-345,-311,381",
                "-661,-816,-575",
                "-876,649,763",
                "-618,-824,-621",
                "553,345,-567",
                "474,580,667",
                "-447,-329,318",
                "-584,868,-557",
                "544,-627,-890",
                "564,392,-477",
                "455,729,728",
                "-892,524,684",
                "-689,845,-530",
                "423,-701,434",
                "7,-33,-71",
                "630,319,-379",
                "443,580,662",
                "-789,900,-551",
                "459,-707,401",
                "",
                "--- scanner 1 ---",
                "686,422,578",
                "605,423,415",
                "515,917,-361",
                "-336,658,858",
                "95,138,22",
                "-476,619,847",
                "-340,-569,-846",
                "567,-361,727",
                "-460,603,-452",
                "669,-402,600",
                "729,430,532",
                "-500,-761,534",
                "-322,571,750",
                "-466,-666,-811",
                "-429,-592,574",
                "-355,545,-477",
                "703,-491,-529",
                "-328,-685,520",
                "413,935,-424",
                "-391,539,-444",
                "586,-435,557",
                "-364,-763,-893",
                "807,-499,-711",
                "755,-354,-619",
                "553,889,-390",
                "",
                "--- scanner 2 ---",
                "649,640,665",
                "682,-795,504",
                "-784,533,-524",
                "-644,584,-595",
                "-588,-843,648",
                "-30,6,44",
                "-674,560,763",
                "500,723,-460",
                "609,671,-379",
                "-555,-800,653",
                "-675,-892,-343",
                "697,-426,-610",
                "578,704,681",
                "493,664,-388",
                "-671,-858,530",
                "-667,343,800",
                "571,-461,-707",
                "-138,-166,112",
                "-889,563,-600",
                "646,-828,498",
                "640,759,510",
                "-630,509,768",
                "-681,-892,-333",
                "673,-379,-804",
                "-742,-814,-386",
                "577,-820,562",
                "",
                "--- scanner 3 ---",
                "-589,542,597",
                "605,-692,669",
                "-500,565,-823",
                "-660,373,557",
                "-458,-679,-417",
                "-488,449,543",
                "-626,468,-788",
                "338,-750,-386",
                "528,-832,-391",
                "562,-778,733",
                "-938,-730,414",
                "543,643,-506",
                "-524,371,-870",
                "407,773,750",
                "-104,29,83",
                "378,-903,-323",
                "-778,-728,485",
                "426,699,580",
                "-438,-605,-362",
                "-469,-447,-387",
                "509,732,623",
                "647,635,-688",
                "-868,-804,481",
                "614,-800,639",
                "595,780,-596",
                "",
                "--- scanner 4 ---",
                "727,592,562",
                "-293,-554,779",
                "441,611,-461",
                "-714,465,-776",
                "-743,427,-804",
                "-660,-479,-426",
                "832,-632,460",
                "927,-485,-438",
                "408,393,-506",
                "466,436,-512",
                "110,16,151",
                "-258,-428,682",
                "-393,719,612",
                "-211,-452,876",
                "808,-476,-593",
                "-575,615,604",
                "-485,667,467",
                "-680,325,-822",
                "-627,-443,-432",
                "872,-547,-609",
                "833,512,582",
                "807,604,487",
                "839,-516,451",
                "891,-625,532",
                "-652,-548,-490",
                "30,-46,-14",
                "",
            };*/

            Parse(data);

            for (int i = 0; i < Scanners.Count; i++)
            {
                for (int j = 0; j < Scanners.Count; j++)
                {
                    if (i != j && Compare(Scanners[i], Scanners[j]))
                    {
                        Console.WriteLine($"Found match: {i} -> {j} with offset {Scanners[j].Offset[0]},{Scanners[j].Offset[1]},{Scanners[j].Offset[2]}");

                        var r = new Result19()
                        {
                            From = i,
                            To = j,
                            Offset = Scanners[j].Offset,
                            Transform = Scanners[j].Transform
                        };

                        Connections.Add(r);
                    }
                }
            }

            foreach (var s in Scanners)
            {
                s.Done = false;
            }

            foreach (var conn in Connections.Where(c => c.From == 0))
            {
                Follow(id, new[] { 0, 0, 0 }, conn);
            }

            foreach (var s in Scanners.Skip(1))
            {
                Console.WriteLine($"Scanner {s.Number}: Offset {s.Offset[0]}, {s.Offset[1]}, {s.Offset[2]}");
            }

            Console.WriteLine($"Number of points in s0: {Scanners[0].Points.Count}");

            var maxDist = Scanners.Max(s1 => Scanners.Max(s2 => ManhattanDist(s1.Offset, s2.Offset)));

            Console.WriteLine($"Largest manhattan distance is {maxDist}");
        }
    }
}
