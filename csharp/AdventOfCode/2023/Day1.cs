using AdventOfCode.Extensions;
using MoreLinq;
using Spectre.Console;

namespace AdventOfCode._2023;

[EventIdentifier(year: 2023, day: 1)]
public class Day1 : Solution
{
    public override Answer Part1(string input)
    {
        return input
            .SplitLines()
            .Select(line =>
                line.ToCharArray()
                    .Where(char.IsDigit)
                    .Select(c => int.Parse(c.ToString()))
                    .ToList())
            .Select(n => $"{n.First()}{n.Last()}")
            .Select(int.Parse)
            .Sum();
    }

    readonly Dictionary<string, string> _numberDict = new()
    {
        ["one"] = "1",
        ["two"] = "2",
        ["three"] = "3",
        ["four"] = "4",
        ["five"] = "5",
        ["six"] = "6",
        ["seven"] = "7",
        ["eight"] = "8",
        ["nine"] = "9",
    };

    private string ReplaceNums(string line)
    {
        for (var i = 0; i < line.Length + 1; i++)
        {
            for (var j = i + 1; j < line.Length + 1; j++)
            {
                if (_numberDict.TryGetValue(line[i..j], out var value))
                {
                    // Replaces twone as 2wone so we can match 2w1 later 
                    return line[..i] +
                           value +
                           ReplaceNums(line[(i + 1)..]);
                }
            }
        }

        return line;
    }

    public override Answer Part2(string input)
    {
        return input
            .SplitLines()
            .Select(ReplaceNums)
            .Select(line =>
                line.ToCharArray()
                    .Where(char.IsDigit)
                    .Select(c => int.Parse(c.ToString()))
                    .ToList()).Select(n => $"{n.First()}{n.Last()}")
            .Select(int.Parse)
            .Sum();
    }
}