using System.Text.RegularExpressions;
using AdventOfCode.Extensions;
using AdventOfCode.Utils;
using AdventOfCode.Utils.Bits;
using MoreLinq.Extensions;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 14)]
public class Day14 : Solution
{
    record BitOperations(
        string Type, // Mask or Write
        string? Mask = null,
        (ulong address, ulong value)? Write = null
    )
    {
        public const string MaskType = "Mask";
        public const string WriteType = "Write";


        public static BitOperations From(string s)
        {
            if (s.Contains("mask"))
            {
                return new BitOperations(Type: MaskType, Mask: s[7..]);
            }

            return new BitOperations(Type: WriteType, Write: ParseWrite(s));
        }

        private static (ulong address, ulong value) ParseWrite(string s)
        {
            var dict = Regex.Match(s, @"\[(?<addr>\d+)\] = (?<value>\d+)").NamedGroups();

            return (dict["addr"].ToUlong(), dict["value"].ToUlong());
        }

        public override string ToString()
        {
            if (Type == MaskType)
            {
                return $"Mask: {Mask}";
            }

            var t = Write!.Value;
            return $"Write [{t.address}] = {t.value}";
        }
    };

    public override Answer Part1(string input)
    {
        var operations = input
            .SplitLines()
            .RemoveEmptyStrings()
            .Select(BitOperations.From);

        var memory = new Dictionary<ulong, ulong>();
        var currentMask = "";

        foreach (var op in operations)
        {
            if (op.Type == BitOperations.MaskType)
            {
                currentMask = op.Mask!;
            }
            else
            {
                var (addr, value) = op.Write!.Value;

                var bits = new UnsignedBitArray(value);

                foreach (var (i, mask) in currentMask.ToCharArray().Index())
                {
                    switch (mask)
                    {
                        case 'X':
                            continue;
                        case '1':
                            bits[35 - i] = true;
                            break;
                        case '0':
                            bits[35 - i] = false;
                            break;
                    }
                }

                memory[addr] = bits.Data;
            }
        }

        return memory.Values.Aggregate(0UL, (current, value) => current + value);
    }

    public override Answer Part2(string input)
    {
        var operations = input
            .SplitLines()
            .RemoveEmptyStrings()
            .Select(BitOperations.From);

        var memory = new Dictionary<ulong, ulong>();
        var currentMask = "";

        foreach (var op in operations)
        {
            if (op.Type == BitOperations.MaskType)
            {
                currentMask = op.Mask!;
            }
            else
            {
                var (addr, value) = op.Write!.Value;
                var bits = new UnsignedBitArray(addr);
                var splitIndices = new List<int>();

                foreach (var (i, mask) in currentMask.ToCharArray().Index())
                {
                    switch (mask)
                    {
                        case 'X':
                            splitIndices.Add(i);
                            break;
                        case '1':
                            bits[35 - i] = true;
                            break;
                        case '0':
                            continue;
                    }
                }

                foreach (var pattern in Combinations.Generate(new[] { false, true }, splitIndices.Count))
                {
                    var newAddr = bits.Clone();
                    for (var n = 0; n < pattern.Length; n++)
                    {
                        var i = splitIndices[n];
                        newAddr[35 - i] = pattern[n];
                    }

                    memory[newAddr.Data] = value;
                }
            }
        }

        return memory.Values.Aggregate(0UL, (current, value) => current + value);
    }
}