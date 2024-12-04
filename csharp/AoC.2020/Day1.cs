using AoC.Shared;
using AoC.Shared.Extensions;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2020;

public class Day1(ITestOutputHelper testOutputHelper) : SolutionBase(2020, 1, false, testOutputHelper)
{
    public override async Task<Answer> Part1(string input)
    {
        var entries = input.ParseAsListOfInts();

        foreach (var (e1, e2) in entries.Cartesian(entries, (i, i1) => (i, i1)))
        {
            if (e1 == e2)
                continue;

            if (e1 + e2 == 2020)
                return e1 * e2;
        }

        return "Failed!";
    }

    public override async Task<Answer> Part2(string input)
    {
        var entries = input.ParseAsListOfInts();

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