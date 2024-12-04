using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit.Abstractions;

namespace AoC._2023;

public class Day2  (ITestOutputHelper outputHelper) : SolutionBase(2023, 2, false, outputHelper)
{
    record Cubes(int R, int G, int B);

    private Cubes ParseRound(string round) => Regex
        .Matches(round, @"(?:(?<amount>\d+) (?<color>blue|green|red))")
        .Aggregate(new Cubes(0, 0, 0),
            (cubes, match) => match.Groups["color"].Value switch
            {
                "red" => cubes with { R = match.Groups["amount"].AsInt() },
                "green" => cubes with { G = match.Groups["amount"].AsInt() },
                "blue" => cubes with { B = match.Groups["amount"].AsInt() },
            });

    private (int Id, Cubes[] Cubes) Parse(string line)
    {
        var p = line.Split(":");
        var game = p[0].Replace("Game", "").Trim().ToInt();
        var rounds = p[1].Split(";").Select(ParseRound).ToArray();
        return (game, rounds);
    }

    public override async Task<Answer> Part1(string input) =>
        input
            .SplitLines()
            .Select(Parse)
            .Where(g => g.Cubes.All(c => c is { R: <= 12, G: <= 13, B: <= 14 }))
            .Select(g => g.Id)
            .Sum();

    public override async Task<Answer> Part2(string input) =>
        input
            .SplitLines()
            .Select(Parse)
            .Select(g => g.Cubes.Aggregate(
                (c, n) => new Cubes(
                    R: Math.Max(c.R, n.R),
                    G: Math.Max(c.G, n.G),
                    B: Math.Max(c.B, n.B))))
            .Select(c => c.R * c.G * c.B)
            .Sum();
}