using AoC.Shared;
using AoC.Shared.DataStructures;
using AoC.Shared.Extensions;
using AoC.Shared.Utils;
using Snapshooter.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace AoC._2024;

public class Day7(ITestOutputHelper testOutputHelper) : SolutionBase(2024, 7, false, testOutputHelper)
{
    record Calibration(long Expected, long[] Factors);

    private Calibration[] Parse(string input) =>
        input
            .SplitLines()
            .Select(line =>
            {
                var parts = line.Split(": ");

                return new Calibration(
                    Expected: long.Parse(parts[0]),
                    Factors: parts[1].ParseAsListOfInts(" ").Select(i => (long)i).ToArray()
                );
            }).ToArray();

    private long Eval(long expected, long[] factors, long curr)
    {
        if (factors.Length == 0)
            return expected == curr ? curr : 0;

        var next = factors[0];
        var rest = factors.Skip(1).ToArray();

        return
            Eval(expected, rest, curr + next) +
            Eval(expected, rest, curr * next);
    }

    public override async Task<Answer> Part1(string input)
    {
        var calibrations = Parse(input);

        var sum = 0L;
        foreach (var c in calibrations)
        {
            var i = Eval(
                c.Expected,
                c.Factors.Skip(1).ToArray(),
                c.Factors.First());

            if (i != 0)
            {
                sum += c.Expected;
            }
        }

        return sum;
    }
    
    private long EvalV2(long expected, long[] factors, long curr)
    {
        if (factors.Length == 0)
            return expected == curr ? curr : 0;

        var next = factors[0];
        var rest = factors.Skip(1).ToArray();

        return
            EvalV2(expected, rest, curr + next) +
            EvalV2(expected, rest, curr * next) +
            EvalV2(expected, rest, Concat(curr, next));
        
        long Concat(long a, long b) => long.Parse(a.ToString() + b);
    }

    public override async Task<Answer> Part2(string input)
    {
        var calibrations = Parse(input);

        var sum = 0L;
        foreach (var c in calibrations)
        {
            var i = EvalV2(
                c.Expected,
                c.Factors.Skip(1).ToArray(),
                c.Factors.First());

            if (i != 0)
            {
                sum += c.Expected;
            }
        }

        return sum;
    }
}