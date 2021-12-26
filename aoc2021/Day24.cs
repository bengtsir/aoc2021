using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    public enum OpType
    {
        Inp,
        Add,
        Mul,
        Div,
        Mod,
        Eql,
    }

    public enum Arg2Type
    {
        Reg,
        Imm
    }

    internal class Instr24
    {
        public OpType Op { get; set; }

        public int Reg1 { get; set; }

        public int Reg2 { get; set; }

        public int Imm { get; set; }

        public Arg2Type RegImm { get; set; }

        private string regNames = "wxyz";

        public Instr24(string s)
        {
            var tokens = s.Split(' ');

            if (tokens.Length < 2)
            {
                throw new Exception("Invalid instruction");
            }

            int args = 1;

            switch (tokens[0])
            {
                case "inp":
                    Op = OpType.Inp;
                    args = 1;
                    break;
                case "add":
                    Op = OpType.Add;
                    args = 2;
                    break;
                case "mul":
                    Op = OpType.Mul;
                    args = 2;
                    break;
                case "div":
                    Op = OpType.Div;
                    args = 2;
                    break;
                case "mod":
                    Op = OpType.Mod;
                    args = 2;
                    break;
                case "eql":
                    Op = OpType.Eql;
                    args = 2;
                    break;
            }

            Reg1 = regNames.IndexOf(tokens[1][0]);

            if (args >= 2)
            {
                if (tokens[2][0] >= 'a' && tokens[2][0] <= 'z')
                {
                    Reg2 = regNames.IndexOf(tokens[2][0]);
                    RegImm = Arg2Type.Reg;
                }
                else
                {
                    Imm = Int32.Parse(tokens[2]);
                    RegImm = Arg2Type.Imm;
                }
            }
        }
    }

    internal class Day24
    {
        private string regNames = "wxyz";

        long[] regs = new long[4];

        public int[] Offsets = new[]
        {
            10, 13, 15, -12, 14, -2, 13,
            -12, 15, 11, -3, -13, -12, -13
        };

        public void Set(string var, long val)
        {
            regs[regNames.IndexOf(var[0])] = val;
        }

        public long Get(string var)
        {
            if (var[0] >= 'a' && var[0] <= 'z')
            {
                return regs[regNames.IndexOf(var[0])];
            }

            return Int32.Parse(var);
        }
        
        bool Run(string[] program, int[] input)
        {
            regs = regNames.Select(c => (long)0).ToArray();

            var inputIndex = 0;

            foreach (var instr in program)
            {
                var tokens = instr.Split(' ');

                if (tokens.Length < 2)
                {
                    break;
                }

                switch (tokens[0])
                {
                    case "inp":

                        Set(tokens[1], input[inputIndex++]);
                        break;
                    case "add":
                        Set(tokens[1], Get(tokens[1]) + Get(tokens[2]));
                        break;
                    case "mul":
                        Set(tokens[1], Get(tokens[1]) * Get(tokens[2]));
                        break;
                    case "div":
                        Set(tokens[1], Get(tokens[1]) / Get(tokens[2]));
                        break;
                    case "mod":
                        Set(tokens[1], Get(tokens[1]) % Get(tokens[2]));
                        break;
                    case "eql":
                        Set(tokens[1], Get(tokens[1]) == Get(tokens[2]) ? 1 : 0);
                        break;
                }
            }

            return Get("z") == 0;
        }

        public List<List<Instr24>> InstructionGroups = new List<List<Instr24>>();

        void ParseInstructions(string[] instrList)
        {
            foreach (var s in instrList)
            {
                var instr = new Instr24(s);
                if (instr.Op == OpType.Inp)
                {
                    InstructionGroups.Add(new List<Instr24>());
                }
                InstructionGroups.Last().Add(instr);
            }
        }

        long RunGroup(List<Instr24> group, long z, int input)
        {
            regs = new long[] { input, 0, 0, z };

            foreach (var instr in group)
            {
                long val2 = instr.Imm;

                if (instr.RegImm == Arg2Type.Reg)
                {
                    val2 = regs[instr.Reg2];
                }

                switch (instr.Op)
                {
                    case OpType.Inp:
                        // Do nothing
                        break;
                    case OpType.Add:
                        regs[instr.Reg1] += val2;
                        break;
                    case OpType.Mul:
                        regs[instr.Reg1] *= val2;
                        break;
                    case OpType.Div:
                        regs[instr.Reg1] /= val2;
                        break;
                    case OpType.Mod:
                        regs[instr.Reg1] %= val2;
                        break;
                    case OpType.Eql:
                        regs[instr.Reg1] = regs[instr.Reg1] == val2 ? 1 : 0;
                        break;

                }
            }

            return regs[3]; // Return z
        }

        internal List<long[]> SearchGroup(List<Instr24> group, ref long[] desiredZ, bool top = false)
        {
            var matches = new List<long[]>();

            var zmin = top ? 0 : -100000;
            var zmax = top ? 1 : 1000000;

            for (int z = zmin; z < zmax; z++)
            {
                for (int w = 1; w < 10; w++)
                {
                    var res = RunGroup(group, z, w);
                    if (desiredZ.Contains(res))
                    {
                        Console.WriteLine($"z = {z} w = {w} res = {res}");
                        matches.Add(new long[]{z, w, res});
                    }
                }
            }

            return matches;
        }

        internal List<List<long[]>> matches = new List<List<long[]>>();
        internal List<long>[] searchedZ = Enumerable.Range(0, 14).Select(i => new List<long>()).ToArray();

        internal void Recurse(int depth)
        {
            var toFind = matches[depth + 1].Select(v => v[0]).Distinct().ToArray();

            Console.WriteLine($"*** Searching depth {depth} for {toFind.Length} values");
            var newm = SearchGroup(InstructionGroups[depth], ref toFind, depth == 0);
            matches[depth].AddRange(newm);

            if (depth > 0)
            {
                Recurse(depth - 1);
            }
        }

        internal void Dive(int depth, long z)
        {
            Console.WriteLine($"Searching d {depth} z {z}");

            for (int w = 1; w < 10; w++)
            {
                var mm = RunGroup(InstructionGroups[depth], z, w);
                matches[depth].Add(new long[]{z, w, mm});
            }

            if (depth < 13)
            {
                foreach (var m in matches[depth])
                {
                    if (!searchedZ[depth].Contains(m[2]))
                    {
                        Dive(depth + 1, m[2]);
                        searchedZ[depth].Add(m[2]);
                    }
                }
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day24.txt");

            ParseInstructions(data);

            var groupNum = InstructionGroups.Count - 1;

            for (int i = 0; i < InstructionGroups.Count; i++)
            {
                matches.Add(new List<long[]>());
            }

            /*
            Dive(0, 0);

            for (int i = 0; i < 14; i++)
            {
                Console.WriteLine($"Depth {i}");
                Console.WriteLine(string.Join(" ", matches[i].Select(v => $"({v[0]},{v[1]},{v[2]})").ToArray()));
            }
            */

            long[] dv = new long[] { 0 };

            var m = SearchGroup(InstructionGroups[groupNum], ref dv, false);
            matches[groupNum] = m;

            Recurse(groupNum - 1);

            for (int i = 0; i < 14; i++)
            {
                Console.WriteLine($"Depth {i}");
                Console.WriteLine(string.Join(" ", matches[i].Select(v => $"({v[0]},{v[1]},{v[2]})").ToArray()));
            }

            long lastZ = 0;
            string digits = "";

            for (int i = 0; i < 14; i++)
            {
                var ml = matches[i].Where(v => v[0] == lastZ);
                var max = ml.Max(v => v[1]);
                var vv = ml.Where(v => v[1] == max).ToList().First();

                Console.WriteLine($"Depth {i}: {vv[0]} - {vv[1]} - {vv[2]}");
                digits += "0123456789"[(int)vv[1]];
                lastZ = vv[2];
            }

            Console.WriteLine($"Final max number is {digits}");

            lastZ = 0;
            digits = "";

            for (int i = 0; i < 14; i++)
            {
                var ml = matches[i].Where(v => v[0] == lastZ);
                var min = ml.Min(v => v[1]);
                var vv = ml.Where(v => v[1] == min).ToList().First();

                Console.WriteLine($"Depth {i}: {vv[0]} - {vv[1]} - {vv[2]}");
                digits += "0123456789"[(int)vv[1]];
                lastZ = vv[2];
            }

            Console.WriteLine($"Final min number is {digits}");

        }
    }
}
