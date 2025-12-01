using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

namespace AoC._2025;

public class Day1(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 1, false, testOutputHelper)
{
    private List<(char, int)> CreateLists(string input)
    {
        return input
            .SplitLines()
            .Select(line => (line[0], int.Parse(line[1..])))
            .ToList();
    }

    private static int TurnDial(int current, char direction, int amount)
    {
        return ((direction == 'L' ? current - amount : current + amount) % 100 + 100) % 100;
    }

    public override async Task<Answer> Part1(string input)
    {
        var list = CreateLists(input);

        var initial = 50;
        var hitZero = 0;
        
        foreach (var (dir, amount) in list)
        {
            initial = TurnDial(initial, dir, amount);
            
            if (initial == 0) hitZero++;
        }

        return hitZero;
    }

    private static int CountZeroHits(int current, char direction, int amount)
    {
        return direction == 'L'
            ? FloorDiv(current - 1, 100) - FloorDiv(current - amount - 1, 100)
            : FloorDiv(current + amount, 100) - FloorDiv(current, 100);

        int FloorDiv(int a, int b) => (int)Floor((double)a / b);
    }


    public override async Task<Answer> Part2(string input)
    {
        var list = CreateLists(input);

        var current = 50;
        var zeroHits = 0;

        foreach (var (dir, amount) in list)
        {
            zeroHits += CountZeroHits(current, dir, amount);
            current = TurnDial(current, dir, amount);
        }

        return zeroHits;
    }
}