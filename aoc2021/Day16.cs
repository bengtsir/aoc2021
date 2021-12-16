using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2021
{
    internal class Packet16
    {
        public int Version { get; private set; }

        public int Id { get; private set; }

        private long value = -1;

        public List<Packet16> Subpackets { get; } = new List<Packet16>();

        public int Parse(ref byte[] bits)
        {
            var versionSum = 0;

            if (bits.Length < 11)
            {
                Version = -1;
                return -1;
            }

            Version = Convert.ToInt32(new string(bits.Take(3).Select(b => "01"[b]).ToArray()), 2);
            Id = Convert.ToInt32(new string(bits.Skip(3).Take(3).Select(b => "01"[b]).ToArray()), 2);

            bits = bits.Skip(6).ToArray();

            versionSum = Version;

            value = -1;

            if (Id == 4)
            {
                List<byte> valbits = new List<byte>();

                bool cont;

                do
                {
                    cont = bits.First() == 1;
                    valbits.AddRange(bits.Skip(1).Take(4));
                    bits = bits.Skip(5).ToArray();
                } while (cont);

                value = Convert.ToInt64(new string(valbits.Select(b => "01"[b]).ToArray()), 2);
            }
            else
            {
                if (bits.First() == 0)
                {
                    var payloadLength =
                        Convert.ToInt32(new string(bits.Skip(1).Take(15).Select(b => "01"[b]).ToArray()), 2);
                    bits = bits.Skip(16).ToArray();
                    var subbits = bits.Take(payloadLength).ToArray();
                    bits = bits.Skip(payloadLength).ToArray();

                    while (subbits.Length > 6)
                    {
                        var subp = new Packet16();
                        versionSum += subp.Parse(ref subbits);
                        Subpackets.Add(subp);
                    }
                }
                else
                {
                    var payloadCount = Convert.ToInt32(new string(bits.Skip(1).Take(11).Select(b => "01"[b]).ToArray()),
                        2);
                    bits = bits.Skip(12).ToArray();

                    for (int i = 0; i < payloadCount; i++)
                    {
                        var subp = new Packet16();
                        versionSum += subp.Parse(ref bits);
                        Subpackets.Add(subp);
                    }
                }
            }

            return versionSum;
        }

        public long Value()
        {
            switch (Id)
            {
                case 0:
                    return Subpackets.Sum(p => p.Value());
                case 1:
                    return Subpackets.Aggregate((long)1, (acc, p) => acc * p.Value());
                case 2:
                    return Subpackets.Min(p => p.Value());
                case 3:
                    return Subpackets.Max(p => p.Value());
                case 4:
                    return value;
                case 5:
                    return Subpackets[0].Value() > Subpackets[1].Value() ? 1 : 0;
                case 6:
                    return Subpackets[0].Value() < Subpackets[1].Value() ? 1 : 0;
                case 7:
                    return Subpackets[0].Value() == Subpackets[1].Value() ? 1 : 0;
            }

            throw new Exception($"Invalid ID code: {Id}");
        }
    }


    internal class Day16
    {
        private byte[] ByteRep(char c)
        {
            switch (c)
            {
                case '0':
                    return new byte[] { 0, 0, 0, 0 };
                case '1':
                    return new byte[] { 0, 0, 0, 1 };
                case '2':
                    return new byte[] { 0, 0, 1, 0 };
                case '3':
                    return new byte[] { 0, 0, 1, 1 };
                case '4':
                    return new byte[] { 0, 1, 0, 0 };
                case '5':
                    return new byte[] { 0, 1, 0, 1 };
                case '6':
                    return new byte[] { 0, 1, 1, 0 };
                case '7':
                    return new byte[] { 0, 1, 1, 1 };
                case '8':
                    return new byte[] { 1, 0, 0, 0 };
                case '9':
                    return new byte[] { 1, 0, 0, 1 };
                case 'A':
                    return new byte[] { 1, 0, 1, 0 };
                case 'B':
                    return new byte[] { 1, 0, 1, 1 };
                case 'C':
                    return new byte[] { 1, 1, 0, 0 };
                case 'D':
                    return new byte[] { 1, 1, 0, 1 };
                case 'E':
                    return new byte[] { 1, 1, 1, 0 };
                case 'F':
                    return new byte[] { 1, 1, 1, 1 };
            }

            return new byte[]{};
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            var bits = data[0].SelectMany(c => ByteRep(c)).ToArray();

            var packets = new List<Packet16>();

            var versionSum = 0;

            while (bits.Length >= 11)
            {
                var p = new Packet16();
                versionSum += p.Parse(ref bits);
                packets.Add(p);
            }

            Console.WriteLine($"Packet version sum is {versionSum}");
        }

        internal long Sumitup(string data)
        {
            var bits = data.SelectMany(c => ByteRep(c)).ToArray();

            var packets = new List<Packet16>();

            long versionSum = 0;

            while (bits.Length >= 11)
            {
                var p = new Packet16();
                versionSum += p.Parse(ref bits);
                packets.Add(p);
            }

            return packets[0].Value();
        }


        public void Part2()
        {
            var testData = new string[]
            {
                "C200B40A82", // finds the sum of 1 and 2, resulting in the value 3.
                "04005AC33890", // finds the product of 6 and 9, resulting in the value 54.
                "880086C3E88112", // finds the minimum of 7, 8, and 9, resulting in the value 7.
                "CE00C43D881120", // finds the maximum of 7, 8, and 9, resulting in the value 9.
                "D8005AC2A8F0", // produces 1, because 5 is less than 15.
                "F600BC2D8F", // produces 0, because 5 is not greater than 15.
                "9C005AC2F8F0", // produces 0, because 5 is not equal to 15.
                "9C0141080250320F1802104A08", // produces 1, because 1 + 3 = 2 * 2.
            };

            var data = File.ReadAllLines(@"data\day16.txt");

            foreach (var s in testData)
            {
                Console.WriteLine($"Testing {s}: {Sumitup(s)}");
            }

            Console.WriteLine($"Real data sum is {Sumitup(data[0])}");
        }
    }
}
