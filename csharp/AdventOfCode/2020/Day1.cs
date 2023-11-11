using MoreLinq;

namespace AdventOfCode._2020;

[EventIdentifier(year: 2020, day: 1)]
public class Day1 : Solution
{
    private int[] Parse(string input) => input
        .Split("\n")
        .Select(int.Parse)
        .ToArray();

    public override Answer Part1(string input)
    {
        var entries = Parse(input);

        foreach (var (e1, e2) in entries.Cartesian(entries, (i, i1) => (i, i1)))
        {
            if (e1 == e2)
                continue;

            if (e1 + e2 == 2020)
                return e1 * e2;
        }

        return "Failed!";
    }

    public override Answer Part2(string input)
    {
        var entries = Parse(input);

        foreach (var (e1, e2, e3) in entries
                     .Cartesian(entries, (e1, e2) => (e1, e2).ToTuple())
                     .Cartesian(entries, (t, e3) => (t.Item1, t.Item2, e3)))
        {
            if (e1 == e2 || e1 == e3)
                continue;

            if (e1 + e2 + e3 == 2020)
                return e1 * e2 * e3;
        }

        return "Failed!";
    }
}