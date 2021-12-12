using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2021
{
    internal class Cave
    {
        public List<Cave> Links { get; } = new List<Cave>();
        public string Name { get; }

        public bool Large => char.IsUpper(Name[0]);
        public bool Small => char.IsLower(Name[0]);

        public bool IsStart => Name == "start";
        public bool IsEnd => Name == "end";

        public bool Mark { get; set; } = false;

        public Cave(string n)
        {
            Name = n;
        }
    }

    internal class Day12
    {
        public int Traverse(Cave c, string path)
        {
            if (c.Mark && c.Small)
            {
                return 0;
            }

            path = $"{path}-{c.Name}";
            Console.WriteLine($"{path}");

            if (c.IsEnd)
            {
                return 1;
            }

            if (c.Small)
            {
                c.Mark = true;
            }

            var links = 0;


            foreach (var link in c.Links)
            {
                links += Traverse(link, path);
            }

            if (c.Small)
            {
                c.Mark = false;
            }

            return links;
        }

        public bool Lifeline = false;
        
        public int Traverse2(Cave c, string path)
        {
            bool usedLifeline = false;

            if (c.Mark && c.Small)
            {
                if (!c.IsStart && !c.IsEnd && !Lifeline)
                {
                    Lifeline = true;
                    usedLifeline = true;
                }
                else
                {
                    return 0;
                }
            }

            path = $"{path}-{c.Name}";
            Console.WriteLine($"{path}");

            if (c.IsEnd)
            {
                return 1;
            }

            if (c.Small)
            {
                c.Mark = true;
            }

            var links = 0;


            foreach (var link in c.Links)
            {
                links += Traverse2(link, path);
            }

            if (c.Small)
            {
                if (usedLifeline)
                {
                    Lifeline = false;
                    // Leave it marked for the other path
                }
                else
                {
                    c.Mark = false;
                }
            }

            return links;
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            var caves = new Dictionary<string, Cave>();

            foreach (var conn in data.Select(x => x.Split('-')))
            {
                foreach (var c in conn)
                {
                    if (!caves.ContainsKey(c))
                    {
                        caves.Add(c, new Cave(c));
                    }
                }

                caves[conn[0]].Links.Add(caves[conn[1]]);
                caves[conn[1]].Links.Add(caves[conn[0]]);
            }

            var nPaths = Traverse(caves["start"], "");

            Console.WriteLine($"Number of paths found: {nPaths}");

        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            var caves = new Dictionary<string, Cave>();

            foreach (var conn in data.Select(x => x.Split('-')))
            {
                foreach (var c in conn)
                {
                    if (!caves.ContainsKey(c))
                    {
                        caves.Add(c, new Cave(c));
                    }
                }

                caves[conn[0]].Links.Add(caves[conn[1]]);
                caves[conn[1]].Links.Add(caves[conn[0]]);
            }

            var nPaths = Traverse2(caves["start"], "");

            Console.WriteLine($"Number of paths found: {nPaths}");
        }

    }
}