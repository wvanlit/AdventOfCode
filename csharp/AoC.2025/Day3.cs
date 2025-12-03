using System.Text.RegularExpressions;
using AoC.Shared;
using AoC.Shared.Extensions;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

namespace AoC._2025;

public class Day3(ITestOutputHelper testOutputHelper) : SolutionBase(2025, 3, true, testOutputHelper)
{
    private List<List<int>> Parse(string input) =>
       input.SplitLines().Select(l => l.ParseAsInts("").ToList()).ToList();

    private int HighestJoltage(List<int> bank, int digits = 2)
    { 
        var max = bank.Max();
       
        if(digits == 1) { return max; }

        var mi = bank.IndexOf(max);
        var prev = HighestJoltage(bank[mi..], digits - 1);
       
        return int.Parse($"{max}{prev}");
    }

    public override async Task<Answer> Part1(string input)
    {
        return Answer.Failed;
    }

    public override async Task<Answer> Part2(string input)
    {
        return Answer.Failed;
    }   
}