using System.Text.RegularExpressions;
using AdventOfCode.Extensions;

namespace AdventOfCode._2020;

[EventIdentifier(2020, 2)]
public class Day2 : Solution
{
    private record Policy(char C, int Min, int Max, string Password)
    {
        public int Count() => Password.Count(c => c == C);

        public bool ValidPart1()
        {
            var count = Count();

            return count <= Max && count >= Min;
        }

        public bool ValidPart2() =>
            // XOR
            (Password[Min - 1] == C) != (Password[Max - 1] == C);
    };

    private static Policy Parse(string input)
    {
        var match = Regex.Match(input, @"(\d+)-(\d+) (\w): (\w+)");
        return new Policy(
            Min: int.Parse(match.Groups[1].Value),
            Max: int.Parse(match.Groups[2].Value),
            C: match.Groups[3].Value[0],
            Password: match.Groups[4].Value
        );
    }

    public override Answer Part1(string input)
    {
        var policies = input.SplitLines().Select(Parse).ToArray();

        return policies.Count(p => p.ValidPart1());
    }

    public override Answer Part2(string input)
    {
        var policies = input.SplitLines().Select(Parse).ToArray();

        return policies.Count(p => p.ValidPart2());
    }
}